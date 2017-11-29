using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System;

[ProtoContract]
public class CS_Data : IExtensible {

    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
    { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }

    [ProtoMember(1)]
    public Int32 Index { get; set; }
    [ProtoMember(2)]
    public Int64 Value { get; set; }
}
