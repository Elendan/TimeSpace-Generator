using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Helpers;
using TimeSpaceGenerator.Objects;
using Button = TimeSpaceGenerator.Objects.Button;

namespace TimeSpaceGenerator.Managers
{
    public class ScriptManager
    {
        #region Members

        private static ScriptManager _instance;

        #endregion

        #region Instantiation

        public ScriptManager()
        {
            Script = new Objects.Script();
            MapMonsters = new List<Monster>();
            Data = new object[5];
        }

        #endregion

        #region Properties

        public bool IsGenerated { get; set; }

        public static ScriptManager Instance => _instance ?? (_instance = new ScriptManager());

        public Objects.Script Script { get; set; }
        
        public Map Map1 { get; set; }

        public Map Map2 { get; set; }

        public Portal Portal1 { get; set; }

        public Monster Monster1 { get; set; }

        public Clock Clock1 { get; set; }

        public List<Monster> MapMonsters { get; set; }

        public int Num1 { get; set; }

        public int Num2 { get; set; }

        public int Num3 { get; set; }

        public short PosX { get; set; }

        public short PosY { get; set; }

        public short IndexX { get; set; }

        public short IndexY { get; set; }

        public bool Flag1 { get; set; }

        public bool Flag2 { get; set; }

        public bool Flag3 { get; set; }

        public object Target { get; set; }

        public string EventName { get; set; }

        public object[] Data { get; set; }

        public string ScriptData { get; set; }

        #endregion

        #region Methods

        //Todo: Review this shit later
        public string RbrPacketManager(string rbrPacket, TextBox name)
        {
            using (var stringreader = new StringReader(rbrPacket))
            {
                string str;
                while ((str = stringreader.ReadLine()) != null)
                {
                    if (Num1 == (byte)1)
                    {
                      //  Script.Info.Label = str;
                    }
                    else
                    {
                        ++Num1;
                        string[] strArray1 = str.Split(' ');
                        if (strArray1.Length > 3 && strArray1[2] == "rbr")
                        {
                            string[] strArray2 = strArray1[6].Split('.');
                            Script.Info.LevelMinimum = byte.Parse(strArray2[0]);
                            Script.Info.LevelMax = byte.Parse(strArray2[1]);
                            Script.Info.SumOfRequired = short.Parse(strArray1[7]);
                            for (int index = 0; index < 5; ++index)
                            {
                                string[] strArray3 = strArray1[8 + index].Split('.');
                                Script.Info.DrawGift.Add(new Item(short.Parse(strArray3[0]), short.Parse(strArray3[1])));
                            }
                            for (int index = 0; index < 2; ++index)
                            {
                                string[] strArray3 = strArray1[13 + index].Split('.');
                                Script.Info.Special.Add(new Item(short.Parse(strArray3[0]), short.Parse(strArray3[1])));
                            }
                            for (int index = 0; index < 3; ++index)
                            {
                                string[] strArray3 = strArray1[15 + index].Split('.');
                                Script.Info.Bonus.Add(new Item(short.Parse(strArray3[0]), short.Parse(strArray3[1])));
                            }
                            Script.Info.Title = strArray1[21];
                            for (int index = 22; index < (strArray1).Count(); ++index)
                            {
                                TimeSpaceInfo info = Script.Info;
                                info.Title = info.Title + " " + strArray1[index];
                            }
                        }
                        name.Text = string.Format("TS - {0}.xml", Script.Info.LevelMinimum);
                    }
                }
            }

            //TODO: Generate the script
            return string.Empty;
        }


        public void AddEvent(object target, string eventName, Event evt)
        {
            switch (target)
            {
                case Monster monster:
                    monster.OnDeathEvents.Add(evt);
                    break;
                case Map map when eventName != "OnCharacterDiscoveringMap":
                    if (eventName != "OnMapClear")
                    {
                        return;
                    }

                    map.OnMapClear.Add(evt);
                    break;
                case Map map:
                    map.OnCharacterDiscoveringMap.Add(evt);
                    break;
                case Button button:
                    button.OnFirstEnable.Add(evt);
                    break;
                default:
                    if (!(target is Portal portal))
                    {
                        return;
                    }

                    portal.OnTraversalEvent.Add(evt);
                    break;
            }
        }


        //TODO: Split and cleanup this shit
        public string GenerateScript()
        {
            string[] space = ScriptHelper.Instance.Space;
            string str1 = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>\r\n<Definition>\r\n" + string.Format("{0}<Globals>\r\n", space[0]) + string.Format("{0}<Label Value=\"{1}\"/>\r\n", space[1], Script.Info.Label) + string.Format("{0}<Title Value=\"{1}\"/>\r\n", space[1], Script.Info.Title) + string.Format("{0}<LevelMinimum Value=\"{1}\"/>\r\n", space[1], Script.Info.LevelMinimum) + string.Format("{0}<LevelMaximum Value=\"{1}\"/>\r\n", space[1], (Script.Info.LevelMax == 0 ? 99 : Script.Info.LevelMax)) + string.Format("{0}<Lives Value=\"{1}\"/>\r\n", space[1], Script.Info.Lives) + string.Format("{0}<RequieredItems>\r\n", space[1]) + string.Format("{0}<Item VNum=\"1012\" Amount=\"{1}\" />\r\n", space[2], (object)Script.Info.SumOfRequired) + string.Format("{0}</RequieredItems>\r\n", space[1]) + string.Format("{0}<DrawItems>\r\n", space[1]);
            foreach (Item obj in Script.Info.DrawGift)
            {
                if (obj.Vnum != -1)
                {
                    str1 += string.Format("{0}<Item VNum=\"{1}\" Amount=\"{2}\"/>\r\n", space[2], obj.Vnum, obj.Amount);
                }
            }
            string str2 = str1 + string.Format("{0}</DrawItems>\r\n", space[1]) + string.Format("{0}<SpecialItems>\r\n", space[1]);
            foreach (Item obj in Script.Info.Special)
            {
                if (obj.Vnum != -1)
                {
                    str2 += string.Format("{0}<Item VNum=\"{1}\" Amount=\"{2}\"/>\r\n", space[2], obj.Vnum, obj.Amount);
                }
            }
            string str3 = str2 + string.Format("{0}</SpecialItems>\r\n", space[1]) + string.Format("{0}<GiftItems>\r\n", space[1]);
            foreach (Item bonu in Script.Info.Bonus)
            {
                if (bonu.Vnum != -1)
                {
                    str3 += string.Format("{0}<Item VNum=\"{1}\" Amount=\"{2}\"/>\r\n", space[2], bonu.Vnum, bonu.Amount);
                }
            }
            string str4 = str3 + string.Format("{0}</GiftItems>\r\n", space[1]) + string.Format("{0}</Globals>\r\n", space[0]) + string.Format("{0}<InstanceEvents>\r\n", space[0]);
            foreach (Map map in Script.Maps.OrderBy(s => s.Id))
            {
                str4 += string.Format("{0}<CreateMap Map=\"{1}\" VNum=\"{2}\" IndexX=\"{3}\" IndexY=\"{4}\">\r\n", space[1], map.Id, map.Vnum, map.IndexX, map.IndexY);
                if (map.OnCharacterDiscoveringMap.Any())
                {
                    str4 += string.Format("{0}<OnCharacterDiscoveringMap>\r\n", space[2]);
                    foreach (Event characterDiscovering in map.OnCharacterDiscoveringMap)
                    {
                        str4 += string.Format("{0}{1}\r\n", space[3], characterDiscovering.SetEvent(3));
                    }
                    List<Event> characterDiscoveringMap = map.OnCharacterDiscoveringMap;
                    if (characterDiscoveringMap.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        str4 += string.Format("{0}<RefreshMapItems/>\r\n", space[3]);
                    }
                    str4 += string.Format("{0}</OnCharacterDiscoveringMap>\r\n", space[2]);
                }

                if (map.OnMapClear.Any())
                {
                    str4 += string.Format("{0}<OnMapClean>\r\n", space[2]);
                    foreach (Event evt in map.OnMapClear)
                        str4 += string.Format("{0}{1}\r\n", space[3], evt.SetEvent(3));
                    List<Event> onMapClear = map.OnMapClear;
                    if (onMapClear.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        str4 += string.Format("{0}<RefreshMapItems/>\r\n", space[3]);
                    }
                    str4 += string.Format("{0}</OnMapClean>\r\n", space[2]);
                }

                if (map.Clock != null)
                {
                    str4 += string.Format("{0}<GenerateClock Value=\"{1}\"/>\r\n", space[2], map.Clock.Time);
                    str4 += string.Format("{0}<StartClock/>\r\n", space[2]);
                }

                if (map.MapClock != null)
                {
                    str4 += string.Format("{0}<GenerateMapClock Value=\"{1}\"/>\r\n", space[2], map.MapClock.Time);
                }

                if (map.MapPortals.Any<Portal>())
                {
                    str4 += string.Format("\r\n{0}<!-- Portals -->\r\n", space[2]);
                    foreach (Portal portal in map.MapPortals)
                    {
                        if (portal.OnTraversalEvent.Any<Event>())
                        {
                            str4 += string.Format("{0}<SpawnPortal IdOnMap=\"{1}\" PositionX=\"{2}\" PositionY=\"{3}\" Type=\"{4}\" ToMap=\"{5}\" ToX =\"{6}\" ToY =\"{7}\">\r\n", (object)space[2], (object)portal.PortalId, (object)portal.PosX, (object)portal.PosY, (object)portal.PortalType, (object)portal.DestMapId, (object)portal.DestX, (object)portal.DestY);
                            str4 += string.Format("{0}<OnTraversal>\r\n", space[3]);
                            foreach (Event evt in portal.OnTraversalEvent)
                            {
                                str4 += string.Format("{0}{1}\r\n", space[4], evt.SetEvent(4));
                            }
                            List<Event> onTraversalEvent = portal.OnTraversalEvent;

                            if (onTraversalEvent.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += string.Format("{0}<RefreshMapItems/>\r\n", space[3]);
                            }
                            str4 += string.Format("{0}</OnTraversal>\r\n", space[3]);
                            str4 += string.Format("{0}</SpawnPortal>\r\n", space[2]);
                        }
                        else
                        {
                            str4 += string.Format("{0}<SpawnPortal IdOnMap=\"{1}\" PositionX=\"{2}\" PositionY=\"{3}\" Type=\"{4}\" ToMap=\"{5}\" ToX =\"{6}\" ToY =\"{7}\"/>\r\n", (object)space[2], (object)portal.PortalId, (object)portal.PosX, (object)portal.PosY, (object)portal.PortalType, (object)portal.DestMapId, (object)portal.DestX, (object)portal.DestY);
                        }
                    }
                }
                if (map.MapNpcs.Any())
                {
                    str4 += string.Format("\r\n{0}<!-- Npcs -->\r\n", (object)space[2]);
                    foreach (Npc npc in map.MapNpcs)
                    {
                        str4 += string.Format("{0}<SummonNpc VNum=\"{1}\" PositionX=\"{2}\" PositionY=\"{3}\" {4} {5}/>\r\n", (object)space[2], (object)npc.Vnum, (object)npc.PosX, (object)npc.PosY, npc.IsMate ? (object)"IsMate=\"True\"" : (object)"", npc.IsProtected ? (object)"IsProtected=\"True\"" : (object)"");
                    }
                }
                if (map.MapButtons.Any())
                {
                    int num = 0;
                    str4 += string.Format("\r\n{0}<!-- Buttons -->\r\n", (object)space[2]);
                    foreach (Button button in map.MapButtons)
                    {
                        if (button.OnFirstEnable.Any<Event>())
                        {
                            str4 += string.Format("{0}<SpawnButton PositionX=\"{1}\" PositionY=\"{2}\" VNumDisabled=\"{3}\" VNumEnabled=\"{4}\" Id=\"{5}\">\r\n", (object)space[2], (object)button.PosX, (object)button.PosY, (object)button.DisableVNum, (object)button.EnableVNum, (object)num++);
                            str4 += string.Format("{0}<OnFirstEnable>\r\n", (object)space[3]);
                            foreach (Event @event in button.OnFirstEnable)
                                str4 += string.Format("{0}{1}\r\n", (object)space[4], (object)@event.SetEvent((byte)4));
                            List<Event> onFirstEnable = button.OnFirstEnable;
                            if (onFirstEnable.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += string.Format("{0}<RefreshMapItems/>\r\n", (object)space[3]);
                            }
                            str4 += string.Format("{0}<RefreshMapItems/>\r\n", (object)space[4]);
                            str4 += string.Format("{0}</OnFirstEnable>\r\n", (object)space[3]);
                            str4 += string.Format("{0}</SpawnButton>\r\n", (object)space[2]);
                        }
                        else
                            str4 += string.Format("{0}<SpawnButton PositionX=\"{1}\" PositionY=\"{2}\" VNumDisabled=\"{3}\" VNumEnabled=\"{4}\" Id=\"{5}\"/>\r\n", (object)space[2], (object)button.PosX, (object)button.PosY, (object)button.DisableVNum, (object)button.EnableVNum, (object)num++);
                    }
                }
                if (map.MapMonsters.Any<Monster>())
                {
                    str4 += string.Format("\r\n{0}<!-- Monsters -->\r\n", (object)space[2]);
                    foreach (Monster monster in map.MapMonsters)
                    {
                        if (!monster.OnDeathEvents.Any<Event>())
                        {
                            str4 += string.Format("{0}<SummonMonster VNum=\"{1}\" PositionX=\"{2}\" PositionY=\"{3}\" {4} {5}/>\r\n", (object)space[2], (object)monster.Vnum, (object)monster.PosX, (object)monster.PosY, monster.IsTarget ? (object)"IsTarget=\"True\"" : (object)"", monster.IsBonus ? (object)"IsBonus=\"True\"" : (object)"");
                        }
                        else
                        {
                            str4 += string.Format("{0}<SummonMonster VNum=\"{1}\" PositionX=\"{2}\" PositionY=\"{3}\"  {4} {5}>\r\n", (object)space[2], (object)monster.Vnum, (object)monster.PosX, (object)monster.PosY, monster.IsTarget ? (object)"IsTarget=\"True\"" : (object)"", monster.IsBonus ? (object)"IsBonus=\"True\"" : (object)"");
                            str4 += string.Format("{0}<OnDeath>\r\n", (object)space[3]);
                            foreach (Event deathEvent in monster.OnDeathEvents)
                            {
                                str4 += string.Format("{0}{1}\r\n", (object)space[4], (object)deathEvent.SetEvent((byte)4));
                            }
                            List<Event> deathEvents = monster.OnDeathEvents;
                            if (deathEvents.Any<Event>(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += string.Format("{0}<RefreshMapItems/>\r\n", (object)space[4]);
                            }
                            str4 += string.Format("{0}</OnDeath>\r\n", (object)space[3]);
                            str4 += string.Format("{0}</SummonMonster>\r\n", (object)space[2]);
                        }
                    }
                }
                str4 += string.Format("{0}</CreateMap>\r\n", (object)space[1]);
            }
            return str4 + string.Format("{0}</InstanceEvents>\r\n", (object)space[0]) + "</Definition>";
        }
        #endregion
    }
}
