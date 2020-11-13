using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace clearpixels.Helpers.database
{
    public static class ContextExtensions
    {
        public static DbCommand GetNewCommand(this DbContext context, string cmdtext, int timeout)
        {
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = cmdtext;
            cmd.CommandTimeout = context.Database.CommandTimeout ?? timeout;

            return cmd;
        }

        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static async Task<int> ExecuteNonQueryAsync2(this DbCommand cmd)
        {
            await cmd.Connection.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows;
        }

        // http://stackoverflow.com/a/19811993/82371
        public static void BulkInsertAll<TEntity>(this DbContext context, IEnumerable<TEntity> entities) where TEntity : class
        {
            var conn = (SqlConnection)context.Database.Connection;

            conn.Open();

            Type t = typeof(TEntity);
            context.Set(t).ToString();
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var workspace = objectContext.MetadataWorkspace;
            var mappings = workspace.GetMappings(objectContext.DefaultContainerName, typeof(TEntity).Name);

            var tableName = context.GetTableName<TEntity>();
            var bulkCopy = new SqlBulkCopy(conn) { DestinationTableName = tableName };

            // Foreign key relations show up as virtual declared 
            // properties and we want to ignore these.
            var properties = t.GetProperties().Where(p => !p.GetGetMethod().IsVirtual).ToArray();
            var table = new DataTable();
            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;

                // Nullable properties need special treatment.
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                // Since we cannot trust the CLR type properties to be in the same order as
                // the table columns we use the SqlBulkCopy column mappings.
                table.Columns.Add(new DataColumn(property.Name, propertyType));
                var clrPropertyName = property.Name;
                var tableColumnName = mappings[property.Name];
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(clrPropertyName, tableColumnName));
            }

            // Add all our entities to our data table
            foreach (var entity in entities)
            {
                var e = entity;
                table.Rows.Add(properties.Select(property => (property.GetValue(e, null)).GetPropertyValue()).ToArray());
            }

            // send it to the server for bulk execution
            bulkCopy.BulkCopyTimeout = 5 * 60;
            bulkCopy.WriteToServer(table);

            conn.Close();
        }

        private static object GetPropertyValue(this object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }

        private static Dictionary<string, string> GetMappings(this MetadataWorkspace workspace, string containerName, string entityName)
        {
            var mappings = new Dictionary<string, string>();
            var storageMapping = workspace.GetItem<GlobalItem>(containerName, DataSpace.CSSpace);
            dynamic entitySetMaps = storageMapping.GetType().InvokeMember(
                "EntitySetMaps",
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                null, storageMapping, null);

            foreach (var entitySetMap in entitySetMaps)
            {
                var typeMappings = entitySetMap.GetArrayList("EntityTypeMappings");
                dynamic typeMapping = typeMappings[0];
                dynamic types = typeMapping.GetArrayList("Types");

                if (types[0].Name == entityName)
                {
                    var fragments = typeMapping.GetArrayList("MappingFragments");
                    var fragment = fragments[0];
                    var properties = fragment.GetArrayList("AllProperties");
                    foreach (var property in properties)
                    {
                        var edmProperty = property.GetProperty("EdmProperty");
                        var columnProperty = property.GetProperty("ColumnProperty");
                        mappings.Add(edmProperty.Name, columnProperty.Name);
                    }
                }
            }

            return mappings;
        }

        private static ArrayList GetArrayList(this object instance, string property)
        {
            var type = instance.GetType();
            var objects = (IEnumerable)type.InvokeMember(
                property,
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, instance, null);
            var list = new ArrayList();
            foreach (var o in objects)
            {
                list.Add(o);
            }
            return list;
        }

        private static dynamic GetProperty(this object instance, string property)
        {
            var type = instance.GetType();
            return type.InvokeMember(property, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, instance, null);
        }
    }
}
