using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PacketData;
public class SocketCallBack  {

    SocketConnectControl socketControl;
    public SocketCallBack(SocketConnectControl scc)
    {
        this.socketControl = scc;
    }

   // public void CMD_SC_PINGRET(Packet packet)
   // {
   //     System.DateTime time1 = GlobalFunction.FromUnixTime(packet.packSCPingRet.sendtime);
   //     System.DateTime time2 = GlobalFunction.FromUnixTime(packet.packSCPingRet.servertime);
   //     ulong timeGap = GlobalFunction.GetTimeStamp() - packet.packSCPingRet.sendtime;
   //     socketControl.pingTime = timeGap;
   //     //Debug.Log("Hour : " + time1.Hour + " min : " + time1.Minute + "  sec : " + time1.Second + "   Time Gap : " + timeGap);
   //     //Debug.Log("Server Hour : " + time2.Hour + " min : " + time2.Minute + "  sec : " + time2.Second);
   // }

   // public void CMD_SC_LOGIN(Packet packet)
   // {
   //     //Debug.Log("CMD_SC_LOGIN");
   //     if (!string.IsNullOrEmpty(packet.packSCLogin.sessionid))
   //     {
   //         socketControl.sessionId = packet.packSCLogin.sessionid;
   //     }
   // }

   // public void CMD_SC_JOIN(Packet packet)
   // {
   //     //Debug.Log("CMD_SC_JOIN");
   //     BallManager.instance.InstantiateMainPlayer(packet.packSCJoin.player);
   // }

   // public void CMD_SC_MOVE(Packet packet)
   // {
   //     //Debug.Log("CMD_SC_MOVE : " + packet.packSCMove.pos.Count);
   //     List<PACVector> scVecList = packet.packSCMove.pos;
   //     //Debug.Log("CMD_SC_MOVE : " + csVecList.Count);
   //     if (scVecList.Count > 0)
   //     {
   //         BallManager.instance.SyncBallMove(scVecList);
   //     }
   // }

   // public void CMD_SC_PLAYER(Packet packet)
   // {
   //     //Debug.Log("CMD_SC_PLAYER");
   //     List<PACPlayerInfo> playerList = packet.packSCPlayer.player;
   //     BallManager.instance.InitCharacterData(playerList);
   // }

   // public void CMD_SC_BALL(Packet packet)
   // {
   //     //Debug.LogWarning("CMD_SC_BALL");
   //     List<PACBall> ballList = packet.packSCBall.ball;
   //     List<PACBallEvent> eventList = packet.packSCBall.eventlist;
   //     BallManager.instance.InstantiateBalls(ballList);
   //     BallManager.instance.ManageBallsEvent(eventList);
   // }

   // public void CMD_SC_STATUS(Packet packet)
   // {
   //     //Debug.LogWarning("CMD_SC_STATUS");
   //     List<PACPlayerStatus> statusList = packet.packSCStatus.pst;
   //     BallManager.instance.SyncPlayerInfo(statusList);
   // }

   // public void CMD_SC_ITEM(Packet packet)
   // {
   //     //Debug.LogWarning("CMD_SC_ITEM");
   //     PacketSCItem pacItem = packet.packSCItem;
   //     List<PACItem> itemList = pacItem.set;
   //     if(BattleManager.instance.battleData.CharacterDic.ContainsKey(BallManager.instance.uid))
   //     {
   //         CharacterInfo cInfo = BattleManager.instance.battleData.CharacterDic[BallManager.instance.uid];
   //         Dictionary<uint, ItemInfo> localBagDic= cInfo.BagDic;
   //         Dictionary<uint, EquipInfo> equipDic = cInfo.C_Equit;
   //         for (int i = 0; i < itemList.Count; i++)
   //         {
   //             ItemInfo itemInfo = new ItemInfo();
   //             itemInfo.ItemId = itemList[i].item;
   //             if(itemList[i].pos < 20)
   //             {
   //                 itemInfo.pos = itemList[i].pos;
   //                 itemInfo.cnt = itemList[i].cnt;
   //                 itemInfo.dur = itemList[i].dur;
   //                 //ItemId 等于0代表该位置没有装备
   //                 if (localBagDic.ContainsKey(itemInfo.pos))
   //                     localBagDic[itemInfo.pos] = itemInfo;
   //                 else
   //                     localBagDic.Add(itemInfo.pos, itemInfo);
   //             }
                
   //             if(itemList[i].pos > 19)
   //             {
   //                 //Debug.Log("itemList[i].pos : " + itemList[i].pos + "  id : " + itemList[i].item);
   //                 switch (itemList[i].pos)
   //                 {
   //                     case 20:
   //                         EquipInfo weapon = new EquipInfo();
   //                         weapon.EquipId = itemList[i].item;
   //                         weapon.hp = (int)itemList[i].dur;
   //                         weapon.EquipNum = (int)itemList[i].cnt;
   //                         if (weapon.EquipId != 0)
   //                             weapon.Kind = GameConfig.Instance.Equitmentconfig[weapon.EquipId].Kind;
   //                         else
   //                             weapon.Kind = 0;
   //                         cInfo.SetWeapons(weapon);
   //                         break;
   //                     case 21:
   //                         EquipInfo helmet = new EquipInfo();
   //                         helmet.EquipId = itemList[i].item;
   //                         helmet.hp = (int)itemList[i].dur;
   //                         helmet.EquipNum = (int)itemList[i].cnt;
   //                         cInfo.SetHelmet(helmet);
   //                         break;
   //                     case 22:
   //                         if (equipDic.ContainsKey(3))
   //                         {
   //                             equipDic[3].EquipId = itemList[i].item;
   //                             equipDic[3].hp = (int)itemList[i].dur;
   //                             equipDic[3].EquipNum = (int)itemList[i].cnt;
   //                         }
   //                         else
   //                         {
   //                             EquipInfo equipInfo = new EquipInfo();
   //                             equipInfo.EquipId = itemList[i].item;
   //                             equipInfo.hp = (int)itemList[i].dur;
   //                             equipInfo.EquipNum = (int)itemList[i].cnt;
   //                             equipDic.Add(3, equipInfo);
   //                         }
   //                         break;
   //                     case 23:
   //                         if (equipDic.ContainsKey(4))
   //                         {
   //                             equipDic[4].EquipId = itemList[i].item;
   //                             equipDic[4].hp = (int)itemList[i].dur;
   //                             equipDic[4].EquipNum = (int)itemList[i].cnt;
   //                         }
   //                         else
   //                         {
   //                             EquipInfo equipInfo = new EquipInfo();
   //                             equipInfo.EquipId = itemList[i].item;
   //                             equipInfo.hp = (int)itemList[i].dur;
   //                             equipInfo.EquipNum = (int)itemList[i].cnt;
   //                             equipDic.Add(4, equipInfo);
   //                         }
   //                         break;
   //                 }
   //             }
   //         }

			////if (MainUIPanels.mainPanel != null && MainUIPanels.mainPanel.ISOpenState == true) {
			////	//Debug.Log ("数据回调");
			////	MainUIPanels.mainPanel.UpdateInfoData ();
			////}

			////if (EquipBoard.equipBoard != null) {
			////	//Debug.Log ("EquipBoard数据回调");
			////	EquipBoard.equipBoard.SetEquipInfo ();
			////}

   //         BallCharacter bc = BallManager.instance.GetCharacter(BallManager.instance.ballId);
   //         //if (bc != null)
   //         //{
   //         //    bc.RefreshCharacterEquip();
   //         //}
   //     }
   // }


   // public void CMD_SC_MSG(Packet packet)
   // {
   //     //Debug.Log("----------------------CMD_SC_MSG------------------------------");
   //     //Debug.Log("from id : " + packet.packSCMsg.fromid + "  name : " + packet.packSCMsg.fromname + "  msg : " + packet.packSCMsg.msg);
   //     BattleManager.instance.messageManager.AddMsg(packet.packSCMsg);
   // }

   // public void CMD_SC_GAMEINFO(Packet packet)
   // {
   //     //Debug.Log("CMD_SC_GAMEINFO");
   //     PacketSCGameInfo gameInfo = packet.packGameInfo;

   //     PACRect rect = gameInfo.board;
   //     Debuger.Log("gameInfo.status = " + gameInfo.status);

   //     if(BattleManager.instance.BattleUI != null)
   //     {
   //         BattleManager.instance.BattleUI.State.text = "State:" + gameInfo.status;
   //     }

   //     if(rect != null)
   //     {
   //         Global.Map_X1 = rect.x1;
   //         Global.Map_X2 = rect.x2;
   //         Global.Map_Y1 = rect.y1;
   //         Global.Map_Y2 = rect.y2;
   //     } 

   //     if (gameInfo.status == "b1")
   //     {
   //         //Debug.Log("nopk ---------- all time : " + gameInfo.alltm + "  curtime : " + gameInfo.curtm);
   //         BattleManager.instance.BattleUI.SetStartCd(gameInfo.alltm, gameInfo.curtm);
   //     }

   //     //开始播放城墙动画然后选择区域
   //     if (gameInfo.status == "b2")
   //     {
   //         //Debug.Log("gameInfo.status = " + gameInfo.status + "   all time: " + gameInfo.alltm + "  curtime : " + gameInfo.curtm);
   //         BattleManager.instance.wallmanager.GetXY(rect.x1, rect.x2, rect.y1, rect.y2);
   //         BattleManager.instance.camerapath.CameraPlay(gameInfo.alltm, gameInfo.curtm);
   //         Global.ItemUp = false;
   //     }

   //     switch(gameInfo.act)
   //     {
   //         case "make":
   //             Debuger.Log("Make");
   //             BattleManager.instance.BattleUI.InitSetCircle(gameInfo.white, gameInfo.blue,gameInfo.alltm, gameInfo.curtm,false);
   //             BattleManager.instance.dieArea.MakeCircle(gameInfo);
   //             break;
   //         case "shrink":
   //             Debuger.Log("Shrink");
   //             BattleManager.instance.dieArea.SetCircleInfo(gameInfo);
   //             BattleManager.instance.BattleUI.InitSetCircle(gameInfo.white, gameInfo.blue, gameInfo.alltm, gameInfo.curtm,true);
   //             //BattleManager.instance.BattleUI.OpenMoveCircle(gameInfo.white, gameInfo.blue);
   //             break;
   //         case "wait":
   //             Debuger.Log("Wait");
   //             BattleManager.instance.BattleUI.InitSetCircle(gameInfo.white, gameInfo.blue, gameInfo.alltm, gameInfo.curtm, false);
   //             BattleManager.instance.dieArea.MakeCircle(gameInfo);
   //             break;
   //     }
   //     BattleManager.instance.BattleUI.SetPlayerCount(gameInfo.live_count, gameInfo.all_count);
   // }

   // public void CMD_SC_GAMEOVER(Packet packet)
   // {
   //     BattleManager.instance.BattleQuit();
   //     BattleManager.instance.BattleUI.InitSettlementPanel(packet.packSCGameOver);
   // }

   // public void CMD_SC_USESKILL(Packet packet)
   // {
   //     PacketSCUseSkill useSkill = packet.packSCUseSkill;
   //     //Debug.Log("useSkill.targetid : " + useSkill.targetid + "   ret " + useSkill.ret + "  posx : " + useSkill.target_x + "   " + useSkill.target_y);
   //     if (useSkill.ret == RetErrorCode.REC_SUCCESS)   //skill successful
   //     {
   //         BallCharacter target = BallManager.instance.GetCharacter(useSkill.targetid);
   //         BallCharacter me = BallManager.instance.GetCharacter(useSkill.sourceid);
   //         if (useSkill.targetid == 0)
   //         {
   //             Vector3 pos = Vector3.zero;
   //             if (!Global.SafeIsland)
   //                 pos = new Vector3(useSkill.target_x, Global.player_y_down, useSkill.target_y);
   //             else
   //                 pos = new Vector3(useSkill.target_x, Global.player_y_up, useSkill.target_y);
   //             if(me != null)
   //                 me.skillManager.UseSkill(useSkill.skillid, pos);
   //             return;
   //         }
   //         if(me != null && target != null)
   //             me.skillManager.UseSkill(useSkill.skillid,Vector3.zero, target);
   //     }
   //     else
   //     {
   //         string tipwords = "技能发动失败";
   //         if (useSkill.ret == RetErrorCode.REC_USESKILL_NOMP)
   //         {
   //             tipwords = "技能需要蓝量不足";
   //             GameObject tip = GameObject.Instantiate(ResourceManager.Load(Global.PREFAB_UI_PATH + "TipSelfPanel") as GameObject);
   //             tip.transform.SetParent(GameObject.Find("Canvas").transform);
   //             tip.transform.localPosition = Vector3.zero;
   //             tip.GetComponent<TipPanel>().InitTipPanel(tipwords);
   //         } else if (useSkill.ret == RetErrorCode.REC_USESKILL_NOCD) 
   //         {
   //             tipwords = "技能CD时间未到";
   //         }
   //         else if (useSkill.ret == RetErrorCode.REC_USESKILL_DIS)
   //         {
   //             tipwords = "技能施法距离不足";
   //             GameObject tip = GameObject.Instantiate(ResourceManager.Load(Global.PREFAB_UI_PATH + "TipSelfPanel") as GameObject);
   //             tip.transform.SetParent( GameObject.Find("Canvas").transform);
   //             tip.transform.localPosition = Vector3.zero;
   //             tip.GetComponent<TipPanel>().InitTipPanel(tipwords);
   //         }
   //     }
   // }


   // public void CMD_SC_BUFFINFO(Packet packet)
   // {
   //     //Debug.LogWarning("---------------------------CMD_SC_BUFFINFO------------------------");
   //     PacketSCBuffInfo scBuffInfo = packet.packSCBuffInfo;
   //     BallManager.instance.SyncBallBuffData(scBuffInfo.bufflist);
   // }
}
