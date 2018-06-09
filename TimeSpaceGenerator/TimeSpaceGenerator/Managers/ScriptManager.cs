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

        public string FileName { get; set; }

        #endregion

        #region Methods

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
            string str1 = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>\r\n<Definition>\r\n" + $"{space[0]}<Globals>\r\n" + $"{space[1]}<Label Value=\"{Script.Info.Label}\"/>\r\n" +
                $"{space[1]}<Title Value=\"{Script.Info.Title}\"/>\r\n" + $"{space[1]}<LevelMinimum Value=\"{Script.Info.LevelMinimum}\"/>\r\n" +
                $"{space[1]}<LevelMaximum Value=\"{(Script.Info.LevelMax == 0 ? 99 : Script.Info.LevelMax)}\"/>\r\n" + $"{space[1]}<Lives Value=\"{Script.Info.Lives}\"/>\r\n" +
                $"{space[1]}<RequieredItems>\r\n" + $"{space[2]}<Item VNum=\"1012\" Amount=\"{Script.Info.SumOfRequired}\" />\r\n" + $"{space[1]}</RequieredItems>\r\n" + $"{space[1]}<DrawItems>\r\n";
            foreach (Item obj in Script.Info.DrawGift)
            {
                if (obj.Vnum != -1)
                {
                    str1 += $"{space[2]}<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n";
                }
            }
            string str2 = str1 + $"{space[1]}</DrawItems>\r\n" + $"{space[1]}<SpecialItems>\r\n";
            foreach (Item obj in Script.Info.Special)
            {
                if (obj.Vnum != -1)
                {
                    str2 += $"{space[2]}<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n";
                }
            }
            string str3 = str2 + $"{space[1]}</SpecialItems>\r\n" + $"{space[1]}<GiftItems>\r\n";
            foreach (Item bonu in Script.Info.Bonus)
            {
                if (bonu.Vnum != -1)
                {
                    str3 += $"{space[2]}<Item VNum=\"{bonu.Vnum}\" Amount=\"{bonu.Amount}\"/>\r\n";
                }
            }
            string str4 = str3 + $"{space[1]}</GiftItems>\r\n" + $"{space[0]}</Globals>\r\n" + $"{space[0]}<InstanceEvents>\r\n";
            foreach (Map map in Script.Maps.OrderBy(s => s.Id))
            {
                str4 += $"{space[1]}<CreateMap Map=\"{map.Id}\" VNum=\"{map.Vnum}\" IndexX=\"{map.IndexX}\" IndexY=\"{map.IndexY}\">\r\n";
                if (map.OnCharacterDiscoveringMap.Any())
                {
                    str4 += $"{space[2]}<OnCharacterDiscoveringMap>\r\n";
                    foreach (Event characterDiscovering in map.OnCharacterDiscoveringMap)
                    {
                        str4 += $"{space[3]}{characterDiscovering.SetEvent(3)}\r\n";
                    }
                    List<Event> characterDiscoveringMap = map.OnCharacterDiscoveringMap;
                    if (characterDiscoveringMap.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        str4 += $"{space[3]}<RefreshMapItems/>\r\n";
                    }
                    str4 += $"{space[2]}</OnCharacterDiscoveringMap>\r\n";
                }

                if (map.OnMapClear.Any())
                {
                    str4 += $"{space[2]}<OnMapClean>\r\n";
                    foreach (Event evt in map.OnMapClear)
                        str4 += $"{space[3]}{evt.SetEvent(3)}\r\n";
                    List<Event> onMapClear = map.OnMapClear;
                    if (onMapClear.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        str4 += $"{space[3]}<RefreshMapItems/>\r\n";
                    }
                    str4 += $"{space[2]}</OnMapClean>\r\n";
                }

                if (map.Clock != null)
                {
                    str4 += $"{space[2]}<GenerateClock Value=\"{map.Clock.Time}\"/>\r\n";
                    str4 += $"{space[2]}<StartClock/>\r\n";
                }

                if (map.MapClock != null)
                {
                    str4 += $"{space[2]}<GenerateMapClock Value=\"{map.MapClock.Time}\"/>\r\n";
                }

                if (map.MapPortals.Any<Portal>())
                {
                    str4 += $"\r\n{space[2]}<!-- Portals -->\r\n";
                    foreach (Portal portal in map.MapPortals)
                    {
                        if (portal.OnTraversalEvent.Any<Event>())
                        {
                            str4 +=
                                $"{space[2]}<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{portal.PortalType}\" ToMap=\"{portal.DestMapId}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\">\r\n";
                            str4 += $"{space[3]}<OnTraversal>\r\n";
                            foreach (Event evt in portal.OnTraversalEvent)
                            {
                                str4 += $"{space[4]}{evt.SetEvent(4)}\r\n";
                            }
                            List<Event> onTraversalEvent = portal.OnTraversalEvent;

                            if (onTraversalEvent.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"{space[3]}<RefreshMapItems/>\r\n";
                            }
                            str4 += $"{space[3]}</OnTraversal>\r\n";
                            str4 += $"{space[2]}</SpawnPortal>\r\n";
                        }
                        else
                        {
                            str4 +=
                                $"{space[2]}<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{portal.PortalType}\" ToMap=\"{portal.DestMapId}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\"/>\r\n";
                        }
                    }
                }
                if (map.MapNpcs.Any())
                {
                    str4 += $"\r\n{space[2]}<!-- Npcs -->\r\n";
                    foreach (Npc npc in map.MapNpcs)
                    {
                        str4 +=
                            $"{space[2]}<SummonNpc VNum=\"{npc.Vnum}\" PositionX=\"{npc.PosX}\" PositionY=\"{npc.PosY}\" {(npc.IsMate ? "IsMate=\"True\"" : "")} {(npc.IsProtected ? "IsProtected=\"True\"" : "")}/>\r\n";
                    }
                }
                if (map.MapButtons.Any())
                {
                    int num = 0;
                    str4 += $"\r\n{space[2]}<!-- Buttons -->\r\n";
                    foreach (Button button in map.MapButtons)
                    {
                        if (button.OnFirstEnable.Any())
                        {
                            str4 +=
                                $"{space[2]}<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\">\r\n";
                            str4 += $"{space[3]}<OnFirstEnable>\r\n";
                            foreach (Event evt in button.OnFirstEnable)
                            {
                                str4 += $"{space[4]}{evt.SetEvent(4)}\r\n";
                            }
                            List<Event> onFirstEnable = button.OnFirstEnable;
                            if (onFirstEnable.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"{space[3]}<RefreshMapItems/>\r\n";
                            }
                            str4 += $"{space[4]}<RefreshMapItems/>\r\n";
                            str4 += $"{space[3]}</OnFirstEnable>\r\n";
                            str4 += $"{space[2]}</SpawnButton>\r\n";
                        }
                        else
                            str4 +=
                                $"{space[2]}<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\"/>\r\n";
                    }
                }
                if (map.MapMonsters.Any<Monster>())
                {
                    str4 += $"\r\n{space[2]}<!-- Monsters -->\r\n";
                    foreach (Monster monster in map.MapMonsters)
                    {
                        if (!monster.OnDeathEvents.Any<Event>())
                        {
                            str4 +=
                                $"{space[2]}<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\" {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}/>\r\n";
                        }
                        else
                        {
                            str4 +=
                                $"{space[2]}<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\"  {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}>\r\n";
                            str4 += $"{space[3]}<OnDeath>\r\n";
                            foreach (Event deathEvent in monster.OnDeathEvents)
                            {
                                str4 += $"{space[4]}{deathEvent.SetEvent((byte)4)}\r\n";
                            }
                            List<Event> deathEvents = monster.OnDeathEvents;
                            if (deathEvents.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"{space[4]}<RefreshMapItems/>\r\n";
                            }
                            str4 += $"{space[3]}</OnDeath>\r\n";
                            str4 += $"{space[2]}</SummonMonster>\r\n";
                        }
                    }
                }
                str4 += $"{space[1]}</CreateMap>\r\n";
            }
            return str4 + $"{space[0]}</InstanceEvents>\r\n" + "</Definition>";
        }
        #endregion
    }
}
