using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public class XmlManager {

    public string SerializeObject(object pObject, Type ty)//序列化数据
    {
        MemoryStream mStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(ty);
        XmlTextWriter xmlTextWriter = new XmlTextWriter(mStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        mStream = (MemoryStream)xmlTextWriter.BaseStream;
        return UTF8ByteArrayToString(mStream.ToArray());
    }

    public void CreatXML(string fileName, string dataString)//创建xml文件并写入
    {
        StreamWriter writer;
        writer = File.CreateText(fileName);
        writer.Write(dataString);
        writer.Close();
    }

    public string UTF8ByteArrayToString(byte[] bytes)//转换数据类型为string
    {
        UTF8Encoding encoding = new UTF8Encoding();
        return encoding.GetString(bytes);
    }

    public object DeserializeObject(string serializedString, Type ty)//反序列化
    {
        XmlSerializer xs = new XmlSerializer(ty);
        MemoryStream mStream = new MemoryStream(StringToUTF8ByteArray(serializedString));
        return xs.Deserialize(mStream);
    }
    
    public string LoadXML(string fileName)//读取数据
    {
        StreamReader reader = File.OpenText(fileName);
        string dataString = reader.ReadToEnd();
        reader.Close();
        return dataString;
    }

    byte[] StringToUTF8ByteArray(string dataString)//转换数据类型为byte
    {
        UTF8Encoding encoding = new UTF8Encoding();
        return encoding.GetBytes(dataString);
    }

    public bool HasFile(string fileName)//文件是否存在
    {
        return File.Exists(fileName);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
