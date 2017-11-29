using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PacketData;
public class PacketCreator  {

    public Packet CsMovePacket()
    {
        Packet packet = new Packet();
        packet.cmd = Packet.EnmCmdType.CMD_CS_MOVE;
        packet.packCSMove = new PacketCSMove();
        return packet;
    }

}
