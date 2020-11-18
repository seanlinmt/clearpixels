//+-------------------------------------------------------------------------------+
//| Copyright (c) 2003 Liping Dai. All rights reserved.                           |
//| Web: www.lipingshare.com                                                      |
//| Email: lipingshare@yahoo.com                                                  |
//|                                                                               |
//| Copyright and Permission Details:                                             |
//| =================================                                             |
//| Permission is hereby granted, free of charge, to any person obtaining a copy  |
//| of this software and associated documentation files (the "Software"), to deal |
//| in the Software without restriction, including without limitation the rights  |
//| to use, copy, modify, merge, publish, distribute, and/or sell copies of the   |
//| Software, subject to the following conditions:                                |
//|                                                                               |
//| 1. Redistributions of source code must retain the above copyright notice, this|
//| list of conditions and the following disclaimer.                              |
//|                                                                               |
//| 2. Redistributions in binary form must reproduce the above copyright notice,  |
//| this list of conditions and the following disclaimer in the documentation     |
//| and/or other materials provided with the distribution.                        |
//|                                                                               |
//| THE SOFTWARE PRODUCT IS PROVIDED �AS IS� WITHOUT WARRANTY OF ANY KIND,        |
//| EITHER EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED         |
//| WARRANTIES OF TITLE, NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR        |
//| A PARTICULAR PURPOSE.                                                         |
//+-------------------------------------------------------------------------------+


using System;
using System.IO;
using System.Collections;
using System.Text;

namespace clearpixels.crypto.Asn1
{
    /// <summary>
    /// IAsn1Node interface.
    /// </summary>
    public interface IAsn1Node
    {
        /// <summary>
        /// Load data from Stream.
        /// </summary>
        /// <param name="xdata"></param>
        /// <returns>true:Succeed; false:failed.</returns>
        bool LoadData(Stream xdata);

        /// <summary>
        /// Save node data into Stream.
        /// </summary>
        /// <param name="xdata">Stream.</param>
        /// <returns>true:Succeed; false:failed.</returns>
        bool SaveData(Stream xdata);
        
        /// <summary>
        /// Get parent node.
        /// </summary>
        Asn1Node ParentNode { get; }

        /// <summary>
        /// Add child node at the end of children list.
        /// </summary>
        /// <param name="xdata">Asn1Node</param>
        void AddChild(Asn1Node xdata);

        /// <summary>
        /// Insert a node in the children list before the pointed index.
        /// </summary>
        /// <param name="xdata">Asn1Node</param>
        /// <param name="index">0 based index.</param>
        int InsertChild(Asn1Node xdata, int index);

        /// <summary>
        /// Insert a node in the children list before the pointed node.
        /// </summary>
        /// <param name="xdata">Asn1Node that will be instered in the children list.</param>
        /// <param name="indexNode">Index node.</param>
		/// <returns>New node index.</returns>
		int InsertChild(Asn1Node xdata, Asn1Node indexNode);

        /// <summary>
        /// Insert a node in the children list after the pointed index.
        /// </summary>
        /// <param name="xdata">Asn1Node</param>
        /// <param name="index">0 based index.</param>
		/// <returns>New node index.</returns>
		int InsertChildAfter(Asn1Node xdata, int index);

        /// <summary>
        /// Insert a node in the children list after the pointed node.
        /// </summary>
        /// <param name="xdata">Asn1Node that will be instered in the children list.</param>
        /// <param name="indexNode">Index node.</param>
		/// <returns>New node index.</returns>
		int InsertChildAfter(Asn1Node xdata, Asn1Node indexNode);

        /// <summary>
        /// Remove a child from children node list by index.
        /// </summary>
        /// <param name="index">0 based index.</param>
        /// <returns>The Asn1Node just removed from the list.</returns>
        Asn1Node RemoveChild(int index);

        /// <summary>
        /// Remove the child from children node list.
        /// </summary>
        /// <param name="node">The node needs to be removed.</param>
        /// <returns></returns>
        Asn1Node RemoveChild(Asn1Node node);

        /// <summary>
        /// Get child node count.
        /// </summary>
        long ChildNodeCount { get; }

        /// <summary>
        /// Retrieve child node by index.
        /// </summary>
        /// <param name="index">0 based index.</param>
        /// <returns>0 based index.</returns>
        Asn1Node GetChildNode(int index);

        /// <summary>
        /// Get descendant node by node path.
        /// </summary>
        /// <param name="nodePath">relative node path that refer to current node.</param>
        /// <returns></returns>
        Asn1Node GetDescendantNodeByPath(string nodePath);

        /// <summary>
        /// Get/Set tag value.
        /// </summary>
        byte Tag{ get; set; }

        /// <summary>
        /// Get tag name.
        /// </summary>
        string TagName{ get; }

        /// <summary>
        /// Get data length. Not included the unused bits byte for BITSTRING.
        /// </summary>
        long DataLength{ get; }

        /// <summary>
        /// Get the length field bytes.
        /// </summary>
        long LengthFieldBytes{ get; }

        /// <summary>
        /// Get data offset.
        /// </summary>
        long DataOffset{ get; }

        /// <summary>
        /// Get unused bits for BITSTRING.
        /// </summary>
        byte UnusedBits{ get; }

        /// <summary>
        /// Get/Set node data by byte[], the data length field content and all the 
        /// node in the parent chain will be adjusted.
        /// </summary>
        byte[] Data { get; set; }

		/// <summary>
		/// Get/Set parseEncapsulatedData. This property will be inherited by the 
		/// child nodes when loading data.
		/// </summary>
		bool ParseEncapsulatedData { get; set; }

        /// <summary>
        /// Get the deepness of the node.
        /// </summary>
        long Deepness { get; }

        /// <summary>
        /// Get the path string of the node.
        /// </summary>
        string Path{ get; }

        /// <summary>
        /// Get the node and all the descendents text description.
        /// </summary>
        /// <param name="startNode">starting node.</param>
        /// <param name="lineLen">line length.</param>
        /// <returns></returns>
        string GetText(Asn1Node startNode, int lineLen);

        /// <summary>
        /// Retrieve the node description.
        /// </summary>
        /// <param name="pureHexMode">true:Return hex string only;
        /// false:Convert to more readable string depending on the node tag.</param>
        /// <returns>string</returns>
        string GetDataStr(bool pureHexMode);

        /// <summary>
        /// Get node label string.
        /// </summary>
        /// <param name="mask">
		/// <code>
		/// SHOW_OFFSET
		/// SHOW_DATA
		/// USE_HEX_OFFSET
		/// SHOW_TAG_NUMBER
		/// SHOW_PATH</code>
		/// </param>
        /// <returns>string</returns>
        string GetLabel(uint mask);

        /// <summary>
        /// Clone a new Asn1Node by current node.
        /// </summary>
        /// <returns>new node.</returns>
        Asn1Node Clone();

        /// <summary>
        /// Clear data and children list.
        /// </summary>
        void ClearAll();
    }

    /// <summary>
    /// Asn1Node, implemented IAsn1Node interface.
    /// </summary>
	public class Asn1Node : IAsn1Node
	{
		// PrivateMembers
		private byte tag;
		private long dataOffset;
		private long dataLength;
		private long lengthFieldBytes;
		private byte[] data;
		private ArrayList childNodeList;
		private byte unusedBits;
		private long deepness;
		private string path = "";
		private const int indentStep = 3;
		private Asn1Node parentNode;
		private bool requireRecalculatePar = true;
		private bool isIndefiniteLength = false;
		private bool parseEncapsulatedData = true;

        /// <summary>
        /// Default Asn1Node text line length.
        /// </summary>
        public const int defaultLineLen = 80;

        /// <summary>
        /// Minium line length.
        /// </summary>
        public const int minLineLen = 60;

		private Asn1Node(Asn1Node parentNode, long dataOffset)
		{
			Init();
			deepness = parentNode.Deepness + 1;
			this.parentNode = parentNode;
			this.dataOffset = dataOffset;
		}

		private void Init()
		{
			childNodeList = new ArrayList();
			data = null;
			dataLength = 0;
			lengthFieldBytes = 0;
			unusedBits = 0;
			tag = Asn1Tag.SEQUENCE | Asn1TagClasses.CONSTRUCTED;
			childNodeList.Clear();
			deepness = 0;
			parentNode = null;
		}

		private string GetHexPrintingStr(Asn1Node startNode, string baseLine, 
			string lStr, int lineLen)
		{
			string nodeStr = "";
			string iStr = GetIndentStr(startNode);
			string dataStr = Asn1Util.ToHexString(data);
			if (dataStr.Length > 0)
			{
				if (baseLine.Length + dataStr.Length < lineLen)
				{
					nodeStr += baseLine + "'" + dataStr + "'";
				}
				else
				{
					nodeStr += baseLine + FormatLineHexString(
						lStr, 
						iStr.Length, 
						lineLen, 
						dataStr
						);
				}
			}
			else
			{
				nodeStr += baseLine;
			}
			return nodeStr + "\r\n";
		}

		private string FormatLineString(string lStr, int indent, int lineLen, string msg)
		{
			string retval = "";
			indent += indentStep;
			int realLen = lineLen - indent;
			int sLen = indent;
			int currentp;
			for (currentp = 0; currentp < msg.Length; currentp += realLen)
			{
				if (currentp+realLen > msg.Length)
				{
					retval += "\r\n" + lStr + Asn1Util.GenStr(sLen,' ') + 
						"'" + msg.Substring(currentp, msg.Length - currentp) + "'";
				}
				else
				{
					retval += "\r\n" + lStr + Asn1Util.GenStr(sLen,' ') + "'" + 
						msg.Substring(currentp, realLen) + "'";
				}
			}
			return retval;
		}

		private string FormatLineHexString(string lStr, int indent, int lineLen, string msg)
		{
			string retval = "";
			indent += indentStep;
			int realLen = lineLen - indent;
			int sLen = indent;
			int currentp;
			for (currentp = 0; currentp < msg.Length; currentp += realLen)
			{
				if (currentp+realLen > msg.Length)
				{
					retval += "\r\n" + lStr + Asn1Util.GenStr(sLen,' ') + 
						msg.Substring(currentp, msg.Length - currentp);
				}
				else
				{
					retval += "\r\n" + lStr + Asn1Util.GenStr(sLen,' ') + 
						msg.Substring(currentp, realLen);
				}
			}
			return retval;
		}

		
		//PublicMembers

		/// <summary>
		/// Constructor, initialize all the members.
		/// </summary>
		public Asn1Node()
		{
			Init();
			dataOffset = 0;
		}

		/// <summary>
		/// Get/Set isIndefiniteLength.
		/// </summary>
		public bool IsIndefiniteLength
		{
			get
			{
				return isIndefiniteLength;
			}
			set
			{
				isIndefiniteLength = value;
			}
		}

        public string GetLabel(uint mask)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clone a new Asn1Node by current node.
        /// </summary>
        /// <returns>new node.</returns>
        public Asn1Node Clone()
        {
            MemoryStream ms = new MemoryStream();
            this.SaveData(ms);
            ms.Position = 0;
            Asn1Node node = new Asn1Node();
            node.LoadData(ms);
            return node;
        }

        /// <summary>
        /// Get/Set tag value.
        /// </summary>
        public byte Tag
        { 
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        /// <summary>
        /// Load data from byte[].
        /// </summary>
        /// <param name="byteData">byte[]</param>
        /// <returns>true:Succeed; false:failed.</returns>
        public bool LoadData(byte[] byteData)
        {
            bool retval = true;
            try
            {
                MemoryStream ms = new MemoryStream(byteData);
                ms.Position = 0;
                retval = LoadData(ms);
                ms.Close();
            }
            catch
            {
                retval = false;
            }
            return retval;
        }

		/// <summary>
		/// Retrieve all the node count in the node subtree.
		/// </summary>
		/// <param name="node">starting node.</param>
		/// <returns>long integer node count in the node subtree.</returns>
		public static long GetDescendantNodeCount(Asn1Node node)
		{
			long count =0;
			count += node.ChildNodeCount;
			for (int i=0; i<node.ChildNodeCount; i++)
			{
				count += GetDescendantNodeCount(node.GetChildNode(i));
			}
			return count;
		}

        /// <summary>
        /// Load data from Stream. Start from current position.
        /// This function sets requireRecalculatePar to false then calls InternalLoadData 
        /// to complish the task.
        /// </summary>
        /// <param name="xdata">Stream</param>
        /// <returns>true:Succeed; false:failed.</returns>
        public bool LoadData(Stream xdata)
        {
			bool retval = false;
            try
            {
                RequireRecalculatePar = false;
				retval = InternalLoadData(xdata);
                return retval;
            }
            finally
            {
                RequireRecalculatePar = true;
                RecalculateTreePar();
            }
        }

		/// <summary>
		/// Call SaveData and return byte[] as result instead stream.
		/// </summary>
		/// <returns></returns>
		public byte[] GetRawData()
		{
			MemoryStream ms = new MemoryStream();
			SaveData(ms);
			byte[] retval = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(retval, 0, (int)ms.Length);
			ms.Close();
			return retval;
		}

		/// <summary>
		/// Get if data is empty.
		/// </summary>
		public bool IsEmptyData
		{
			get
			{
				if (data == null) return true;
				if (data.Length < 1)
					return true;
				else
					return false;
			}
		}

        /// <summary>
        /// Save node data into Stream.
        /// </summary>
        /// <param name="xdata">Stream.</param>
        /// <returns>true:Succeed; false:failed.</returns>
        public bool SaveData(Stream xdata)
        {
            bool retval = true;
            long nodeCount = ChildNodeCount;
            xdata.WriteByte(tag);
            int tmpLen = Asn1Util.DERLengthEncode(xdata, (ulong) dataLength);
            if ((tag) == Asn1Tag.BIT_STRING)
            {
                xdata.WriteByte(unusedBits);
            }
            if (nodeCount==0)
            {
                if (data != null)
                {
                    xdata.Write(data, 0, data.Length);
                }
            }
            else
            {
                Asn1Node tempNode;
                int i;
                for (i=0; i<nodeCount; i++)
                {
                    tempNode = GetChildNode(i);
                    retval = tempNode.SaveData(xdata);
                }
            }
            return retval;
        }

        /// <summary>
        /// Clear data and children list.
        /// </summary>
        public void ClearAll()
        {
            data = null;
            Asn1Node tempNode;
            for (int i=0; i<childNodeList.Count; i++)
            {
                tempNode = (Asn1Node) childNodeList[i];
                tempNode.ClearAll();
            }
            childNodeList.Clear();
            RecalculateTreePar();
        }

        
        /// <summary>
        /// Add child node at the end of children list.
        /// </summary>
        /// <param name="xdata">the node that will be add in.</param>
        public void AddChild(Asn1Node xdata)
        {
            childNodeList.Add(xdata);
            RecalculateTreePar();
        }

        /// <summary>
        /// Insert a node in the children list before the pointed index.
        /// </summary>
        /// <param name="xdata">Asn1Node</param>
        /// <param name="index">0 based index.</param>
		/// <returns>New node index.</returns>
		public int InsertChild(Asn1Node xdata, int index)
        {
            childNodeList.Insert(index, xdata);
            RecalculateTreePar();
			return index;
        }

        /// <summary>
        /// Insert a node in the children list before the pointed node.
        /// </summary>
        /// <param name="xdata">Asn1Node that will be instered in the children list.</param>
        /// <param name="indexNode">Index node.</param>
		/// <returns>New node index.</returns>
		public int InsertChild(Asn1Node xdata, Asn1Node indexNode)
        {
			int index = childNodeList.IndexOf(indexNode);
            childNodeList.Insert(index, xdata);
            RecalculateTreePar();
			return index;
        }

        /// <summary>
        /// Insert a node in the children list after the pointed node.
        /// </summary>
        /// <param name="xdata">Asn1Node</param>
        /// <param name="indexNode">Index node.</param>
		/// <returns>New node index.</returns>
		public int InsertChildAfter(Asn1Node xdata, Asn1Node indexNode)
        {
			int index = childNodeList.IndexOf(indexNode)+1;
            childNodeList.Insert(index, xdata);
            RecalculateTreePar();
			return index;
        }

        /// <summary>
        /// Insert a node in the children list after the pointed node.
        /// </summary>
        /// <param name="xdata">Asn1Node that will be instered in the children list.</param>
        /// <param name="index">0 based index.</param>
		/// <returns>New node index.</returns>
		public int InsertChildAfter(Asn1Node xdata, int index)
        {
			int xindex = index+1;
            childNodeList.Insert(xindex, xdata);
            RecalculateTreePar();
			return xindex;
        }

        /// <summary>
        /// Remove a child from children node list by index.
        /// </summary>
        /// <param name="index">0 based index.</param>
        /// <returns>The Asn1Node just removed from the list.</returns>
        public Asn1Node RemoveChild(int index)
        {
            Asn1Node retval = null;
            if (index < (childNodeList.Count - 1))
            {
                retval = (Asn1Node) childNodeList[index+1];
            }
            childNodeList.RemoveAt(index);
            if (retval == null)
            {
                if (childNodeList.Count > 0)
                {
                    retval = (Asn1Node) childNodeList[childNodeList.Count-1];
                }
                else
                {
                    retval = this;
                }
            }
            RecalculateTreePar();
            return retval;
        }

        /// <summary>
        /// Remove the child from children node list.
        /// </summary>
        /// <param name="node">The node needs to be removed.</param>
        /// <returns></returns>
        public Asn1Node RemoveChild(Asn1Node node)
        {
            Asn1Node retval = null;
            int i = childNodeList.IndexOf(node);
            retval = RemoveChild(i);
            return retval;
        }

        /// <summary>
        /// Get child node count.
        /// </summary>
        public long ChildNodeCount
        {
            get
            {
                return childNodeList.Count;
            }
        }

        /// <summary>
        /// Retrieve child node by index.
        /// </summary>
        /// <param name="index">0 based index.</param>
        /// <returns>0 based index.</returns>
        public Asn1Node GetChildNode(int index)
        {
            Asn1Node retval = null;
            if (index < ChildNodeCount)
            {
                retval = (Asn1Node) childNodeList[index];
            }
            return retval;
        }



        /// <summary>
        /// Get tag name.
        /// </summary>
        public string TagName
        {
            get
            {
                return Asn1Util.GetTagName(tag);
            }
        }

        /// <summary>
        /// Get parent node.
        /// </summary>
        public Asn1Node ParentNode
        {
            get
            {
                return parentNode;
            }
        }

        /// <summary>
        /// Get the path string of the node.
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }

        public string GetText(Asn1Node startNode, int lineLen)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve the node description.
        /// </summary>
        /// <param name="pureHexMode">true:Return hex string only;
        /// false:Convert to more readable string depending on the node tag.</param>
        /// <returns>string</returns>
        public string GetDataStr(bool pureHexMode)
        {
            const int lineLen = 32;
            string dataStr = "";
            if (pureHexMode)
            {
                dataStr = Asn1Util.FormatString(Asn1Util.ToHexString(data), lineLen, 2);
            }
            else
            {
                switch (tag)
                {
                    case Asn1Tag.BIT_STRING:
                        dataStr = Asn1Util.FormatString(Asn1Util.ToHexString(data), lineLen, 2);
                        break;
                    case Asn1Tag.OBJECT_IDENTIFIER:
                        Oid xoid = new Oid();
                        dataStr = xoid.Decode(new MemoryStream(data));
                        break;
                    case Asn1Tag.RELATIVE_OID:
                        RelativeOid roid = new RelativeOid();
                        dataStr = roid.Decode(new MemoryStream(data));
                        break;
                    case Asn1Tag.PRINTABLE_STRING:
                    case Asn1Tag.IA5_STRING:
                    case Asn1Tag.UNIVERSAL_STRING:
                    case Asn1Tag.VISIBLE_STRING:
                    case Asn1Tag.NUMERIC_STRING:
                    case Asn1Tag.UTC_TIME:
                    case Asn1Tag.BMPSTRING:
					case Asn1Tag.GENERAL_STRING:
                    case Asn1Tag.GENERALIZED_TIME:
                        dataStr = Asn1Util.BytesToString(data);
                        break;
					case Asn1Tag.UTF8_STRING:
						UTF8Encoding utf8 = new UTF8Encoding();
						dataStr = utf8.GetString(data);
						break;
					case Asn1Tag.INTEGER:
                        dataStr = Asn1Util.FormatString(Asn1Util.ToHexString(data), lineLen, 2);
                        break;
                    default:
                        if ((tag & Asn1Tag.TAG_MASK) == 6  || // Visible string for certificate
                            Asn1Util.IsAsciiString(Data))
                        {
                            dataStr = Asn1Util.BytesToString(data);
                        }
                        else
                        {
                            dataStr = Asn1Util.FormatString(Asn1Util.ToHexString(data), lineLen, 2);
                        }
                        break;
                };
            }
            return dataStr;
        }

        /// <summary>
        /// Get data length. Not included the unused bits byte for BITSTRING.
        /// </summary>
        public long DataLength
        {
            get
            {
                return dataLength;
            }
        }

        /// <summary>
        /// Get the length field bytes.
        /// </summary>
        public long LengthFieldBytes
        {
            get
            {
                return lengthFieldBytes;
            }
        }

        /// <summary>
        /// Get/Set node data by byte[], the data length field content and all the 
        /// node in the parent chain will be adjusted.
        /// <br></br>
        /// It return all the child data for constructed node.
        /// </summary>
        public byte[] Data
        {
            get
            {
				MemoryStream xdata = new MemoryStream();
				long nodeCount = ChildNodeCount;
				if (nodeCount==0)
				{
					if (data != null)
					{
						xdata.Write(data, 0, data.Length);
					}
				}
				else
				{
					Asn1Node tempNode;
					for (int i=0; i<nodeCount; i++)
					{
						tempNode = GetChildNode(i);
						tempNode.SaveData(xdata);
					}
				}
				byte[] tmpData = new byte[xdata.Length];
				xdata.Position = 0;
				xdata.Read(tmpData, 0, (int)xdata.Length);
				xdata.Close();
				return tmpData;
            }
            set
            {
                SetData(value);
            }
        }

        /// <summary>
        /// Get the deepness of the node.
        /// </summary>
        public long Deepness
        {
            get
            {
                return deepness;
            }
        }

        /// <summary>
        /// Get data offset.
        /// </summary>
        public long DataOffset
        {
            get
            {
                return dataOffset;
            }
        }

        /// <summary>
        /// Get unused bits for BITSTRING.
        /// </summary>
        public byte UnusedBits
        {
            get
            {
                return unusedBits;
            }
            set
            {
                unusedBits = value;
            }
        }


        /// <summary>
        /// Get descendant node by node path.
        /// </summary>
        /// <param name="nodePath">relative node path that refer to current node.</param>
        /// <returns></returns>
        public Asn1Node GetDescendantNodeByPath(string nodePath)
        {
            Asn1Node retval = this;
            if (nodePath == null) return retval;
            nodePath = nodePath.TrimEnd().TrimStart();
            if (nodePath.Length<1) return retval;
            string[] route = nodePath.Split('/');
            try
            {
                for (int i = 1; i<route.Length; i++)
                {
                    retval = retval.GetChildNode(Convert.ToInt32(route[i]));
                }
            }
            catch
            {
                retval = null;
            }
            return retval;
        }

		/// <summary>
		/// Get node by OID.
		/// </summary>
		/// <param name="oid">OID.</param>
		/// <param name="startNode">Starting node.</param>
		/// <returns>Null or Asn1Node.</returns>
		static public Asn1Node GetDecendantNodeByOid(string oid, Asn1Node startNode)
		{
			Asn1Node retval = null;
            Oid xoid = new Oid();
			for (int i = 0; i<startNode.ChildNodeCount; i++)
			{
				Asn1Node childNode = startNode.GetChildNode(i);
				int tmpTag = childNode.tag & Asn1Tag.TAG_MASK;
				if (tmpTag == Asn1Tag.OBJECT_IDENTIFIER)
				{
					if (oid == xoid.Decode(childNode.Data))
					{
						retval = childNode;
						break;
					}
				}
				retval = GetDecendantNodeByOid(oid, childNode);
				if (retval != null) break;
			}
			return retval;
		}
        
        /// <summary>
        /// Constant of tag field length.
        /// </summary>
        public const int TagLength = 1;

        /// <summary>
        /// Constant of unused bits field length.
        /// </summary>
        public const int BitStringUnusedFiledLength = 1;

        /// <summary>
        /// Tag text generation mask definition.
        /// </summary>
        public class TagTextMask
        {
            /// <summary>
            /// Show offset.
            /// </summary>
            public const uint SHOW_OFFSET			= 0x01;

            /// <summary>
            /// Show decoded data.
            /// </summary>
            public const uint SHOW_DATA			    = 0x02;

            /// <summary>
            /// Show offset in hex format.
            /// </summary>
            public const uint USE_HEX_OFFSET		= 0x04;

            /// <summary>
            /// Show tag.
            /// </summary>
            public const uint SHOW_TAG_NUMBER		= 0x08;

            /// <summary>
            /// Show node path.
            /// </summary>
            public const uint SHOW_PATH             = 0x10;
        }

        /// <summary>
        /// Set/Get requireRecalculatePar. RecalculateTreePar function will not do anything
        /// if it is set to false. 
        /// </summary>
        protected bool RequireRecalculatePar
        {
            get
            {
                return requireRecalculatePar;
            }
            set
            {
                requireRecalculatePar = value;
            }
        }

		//ProtectedMembers

        /// <summary>
        /// Find root node and recalculate entire tree length field, 
        /// path, offset and deepness.
        /// </summary>
        protected void RecalculateTreePar()
        {
            if (!requireRecalculatePar) return;
            Asn1Node rootNode;
            for (rootNode = this; rootNode.ParentNode != null;)
            {
                rootNode = rootNode.ParentNode;
            }
            ResetBranchDataLength(rootNode);
            rootNode.dataOffset = 0;
            rootNode.deepness = 0;
            long subOffset = rootNode.dataOffset + TagLength + rootNode.lengthFieldBytes;
            ResetChildNodePar(rootNode, subOffset);
        }

        /// <summary>
        /// Recursively set all the node data length.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>node data length.</returns>
        protected static long ResetBranchDataLength(Asn1Node node)
        {
            long retval = 0;
            long childDataLength = 0;
            if (node.ChildNodeCount < 1)
            {
                if (node.data != null)
                    childDataLength += node.data.Length;
            }
            else
            {
                for (int i=0; i<node.ChildNodeCount; i++)
                {
                    childDataLength += ResetBranchDataLength(node.GetChildNode(i));
                }
            }
            node.dataLength = childDataLength;
            if (node.tag == Asn1Tag.BIT_STRING)
                node.dataLength += BitStringUnusedFiledLength;
            ResetDataLengthFieldWidth(node);
            retval = node.dataLength + TagLength + node.lengthFieldBytes;
            return retval;
        }

        /// <summary>
        /// Encode the node data length field and set lengthFieldBytes and dataLength.
        /// </summary>
        /// <param name="node">The node needs to be reset.</param>
        protected static void ResetDataLengthFieldWidth(Asn1Node node)
        {
            MemoryStream tempStream = new MemoryStream();
            Asn1Util.DERLengthEncode(tempStream, (ulong) node.dataLength);
            node.lengthFieldBytes = tempStream.Length;
            tempStream.Close();
        }

        /// <summary>
        /// Recursively set all the child parameters, except dataLength.
        /// dataLength is set by ResetBranchDataLength.
        /// </summary>
        /// <param name="xNode">Starting node.</param>
        /// <param name="subOffset">Starting node offset.</param>
        protected void ResetChildNodePar(Asn1Node xNode, long subOffset)
        {
            int i;
            if (xNode.tag == Asn1Tag.BIT_STRING)
            {
                subOffset++;
            }
            Asn1Node tempNode;
            for (i=0; i<xNode.ChildNodeCount; i++)
            {
                tempNode = xNode.GetChildNode(i);
                tempNode.parentNode = xNode;
                tempNode.dataOffset = subOffset;
                tempNode.deepness = xNode.deepness + 1;
                tempNode.path = xNode.path + '/' + i.ToString();
                subOffset += TagLength + tempNode.lengthFieldBytes;
                ResetChildNodePar(tempNode, subOffset);
                subOffset += tempNode.dataLength;
            }
        }


        /// <summary>
        /// Generate the node indent string.
        /// </summary>
        /// <param name="startNode">The node.</param>
        /// <returns>Text string.</returns>
        protected string GetIndentStr(Asn1Node startNode)
        {
            string retval = "";
            long startLen = 0;
            if (startNode!=null)
            {
                startLen = startNode.Deepness;
            }
            for (long i = 0; i<deepness - startLen; i++)
            {
                retval += "   ";
            }
            return retval;
        }

        /// <summary>
        /// Decode ASN.1 encoded node Stream data.
        /// </summary>
        /// <param name="xdata">Stream data.</param>
        /// <returns>true:Succeed, false:Failed.</returns>
        protected bool GeneralDecode(Stream xdata)
        {
            bool retval = false;
            long nodeMaxLen;
            nodeMaxLen = xdata.Length - xdata.Position;
            tag = (byte) xdata.ReadByte();
            long start, end;
            start = xdata.Position;
            dataLength = Asn1Util.DerLengthDecode(xdata, ref isIndefiniteLength);
            if (dataLength < 0) return retval; // Node data length can not be negative.
            end = xdata.Position;
            lengthFieldBytes = end - start;
            if (nodeMaxLen < (dataLength + TagLength + lengthFieldBytes))
            {
                return retval;
            }
            if ( ParentNode == null || ((ParentNode.tag & Asn1TagClasses.CONSTRUCTED) == 0))
            {
                if ((tag & Asn1Tag.TAG_MASK)<=0 || (tag & Asn1Tag.TAG_MASK)>0x1E) return retval;
            }
            if (tag == Asn1Tag.BIT_STRING)
            {
                // First byte of BIT_STRING is unused bits.
                // BIT_STRING data does not include this byte.

                // Fixed by Gustaf Bj�rklund.
                if (dataLength < 1) return retval; // We cannot read less than 1 - 1 bytes.

                unusedBits = (byte) xdata.ReadByte();
                data = new byte[dataLength-1];
                xdata.Read(data, 0, (int)(dataLength-1) );
            }
            else
            {
                data = new byte[dataLength];
                xdata.Read(data, 0, (int)(dataLength) );
            }
            retval = true;
            return retval;
        }

        /// <summary>
        /// Decode ASN.1 encoded complex data type Stream data.
        /// </summary>
        /// <param name="xdata">Stream data.</param>
        /// <returns>true:Succeed, false:Failed.</returns>
        protected bool ListDecode(Stream xdata)
        {
            bool retval = false;
            long originalPosition = xdata.Position;
            long childNodeMaxLen;
            try
            {
                childNodeMaxLen = xdata.Length - xdata.Position;
                tag = (byte) xdata.ReadByte();
                long start, end, offset;
                start = xdata.Position;
                dataLength = Asn1Util.DerLengthDecode(xdata, ref isIndefiniteLength);
                if (dataLength<0 || childNodeMaxLen<dataLength)
                {
                    return retval;
                }
                end = xdata.Position;
                lengthFieldBytes = end - start;
                offset = dataOffset + TagLength + lengthFieldBytes;
                Stream secData;
                byte[] secByte;
                if (tag == Asn1Tag.BIT_STRING)
                {
                    // First byte of BIT_STRING is unused bits.
                    // BIT_STRING data does not include this byte.
                    unusedBits = (byte) xdata.ReadByte();
                    dataLength--;
                    offset++;
                }
                if (dataLength <= 0) return retval; // List data length cann't be zero.
                secData = new MemoryStream((int)dataLength);
                secByte = new byte[dataLength];
                xdata.Read(secByte, 0, (int) (dataLength));
                if (tag == Asn1Tag.BIT_STRING) dataLength++;
                secData.Write(secByte, 0, secByte.Length);
                secData.Position = 0;
                while(secData.Position<secData.Length)
                {
                    Asn1Node node = new Asn1Node(this, offset);
					node.parseEncapsulatedData = this.parseEncapsulatedData;
                    start = secData.Position;
                    if (!node.InternalLoadData(secData)) return retval;
                    AddChild(node);
                    end = secData.Position;
                    offset += end - start;
                }
                retval = true;
            }
            finally
            {
                if (!retval)
                {
                    xdata.Position = originalPosition;
                    ClearAll();
                }
            }
            return retval;
        }

        /// <summary>
        /// Set the node data and recalculate the entire tree parameters.
        /// </summary>
        /// <param name="xdata">byte[] data.</param>
        protected void SetData(byte[] xdata)
        {
			if (childNodeList.Count > 0)
			{
				throw new Exception("Constructed node can't hold simple data.");
			}
			else
			{
				data = xdata;
				if (data != null)
					dataLength = data.Length;
				else
					dataLength = 0;
				RecalculateTreePar();
			}
        }

        /// <summary>
        /// Load data from Stream. Start from current position.
        /// </summary>
        /// <param name="xdata">Stream</param>
        /// <returns>true:Succeed; false:failed.</returns>
        protected bool InternalLoadData(Stream xdata)
        {
            bool retval = true;
            ClearAll();
            byte xtag; 
            long curPosition = xdata.Position;
            xtag = (byte) xdata.ReadByte();
            xdata.Position = curPosition;
            int maskedTag = xtag & Asn1Tag.TAG_MASK;
            if (((xtag & Asn1TagClasses.CONSTRUCTED) != 0) 
                || (parseEncapsulatedData 
				&& ((maskedTag == Asn1Tag.BIT_STRING)
                || (maskedTag == Asn1Tag.EXTERNAL)
                || (maskedTag == Asn1Tag.GENERAL_STRING)
                || (maskedTag == Asn1Tag.GENERALIZED_TIME)
                || (maskedTag == Asn1Tag.GRAPHIC_STRING)
                || (maskedTag == Asn1Tag.IA5_STRING)
                || (maskedTag == Asn1Tag.OCTET_STRING)
                || (maskedTag == Asn1Tag.PRINTABLE_STRING)
                || (maskedTag == Asn1Tag.SEQUENCE)
                || (maskedTag == Asn1Tag.SET)
                || (maskedTag == Asn1Tag.T61_STRING)
                || (maskedTag == Asn1Tag.UNIVERSAL_STRING)
                || (maskedTag == Asn1Tag.UTF8_STRING)
                || (maskedTag == Asn1Tag.VIDEOTEXT_STRING)
                || (maskedTag == Asn1Tag.VISIBLE_STRING)))
                )
            {
                if (!ListDecode(xdata))
                {
                    if (!GeneralDecode(xdata))
                    {
                        retval = false;
                    }
                }
            }
            else
            {
                if (!GeneralDecode(xdata)) retval = false;
            }
            return retval;
        }

		/// <summary>
		/// Get/Set parseEncapsulatedData. This property will be inherited by the 
		/// child nodes when loading data.
		/// </summary>
		public bool ParseEncapsulatedData 
		{ 
			get
			{
				return parseEncapsulatedData;
			}
			set
			{
				if (parseEncapsulatedData == value) return;
				byte[] tmpData = Data;
				parseEncapsulatedData = value;
				ClearAll();
				if ((tag & Asn1TagClasses.CONSTRUCTED) != 0 || parseEncapsulatedData)
				{
					MemoryStream ms = new MemoryStream(tmpData);
					ms.Position = 0;
					bool isLoaded = true;
					while(ms.Position < ms.Length)
					{
						Asn1Node tempNode = new Asn1Node();
						tempNode.ParseEncapsulatedData = parseEncapsulatedData;
						if (!tempNode.LoadData(ms))
						{
							ClearAll();
							isLoaded = false;
							break;
						}
						AddChild(tempNode);
					}
					if (!isLoaded)
					{
						Data = tmpData;
					}
				}
				else
				{
					Data = tmpData;
				}
			}
		}

    }

}
