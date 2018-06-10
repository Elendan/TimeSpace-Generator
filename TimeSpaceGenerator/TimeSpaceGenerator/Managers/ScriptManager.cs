using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Objects;

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
            Script = new Script();
            MapMonsters = new List<Monster>();
            Doc = new XmlDocument();
            Data = new object[5];
        }

        #endregion

        #region Properties

        public bool IsGenerated { get; set; }

        public static ScriptManager Instance => _instance ?? (_instance = new ScriptManager());

        public Script Script { get; set; }

        public Map Map1 { get; set; }

        public Map Map2 { get; set; }

        public Portal Portal1 { get; set; }

		public Portal LastPortal { get; set; }

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

        public bool LabelSet { get; set; }

        public XmlDocument Doc { get; set; }

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
            string str1 = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>\r\n<Definition>\r\n" + $"<Globals>\r\n" + $"<Label Value=\"{Script.Info.Label}\"/>\r\n" +
                $"<Title Value=\"{Script.Info.Title}\"/>\r\n" + $"<LevelMinimum Value=\"{Script.Info.LevelMinimum}\"/>\r\n" +
                $"<LevelMaximum Value=\"{(Script.Info.LevelMax == 0 ? 99 : Script.Info.LevelMax)}\"/>\r\n" + $"<Lives Value=\"{Script.Info.Lives}\"/>\r\n" +
                $"<RequieredItems>\r\n" + $"<Item VNum=\"1012\" Amount=\"{Script.Info.SumOfRequired}\" />\r\n" + $"</RequieredItems>\r\n" + $"<DrawItems>\r\n";
            foreach (Item obj in Script.Info.DrawGift)
            {
                if (obj.Vnum != -1)
                {
                    str1 += $"<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n";
                }
            }

            string str2 = str1 + $"</DrawItems>\r\n" + $"<SpecialItems>\r\n";
            foreach (Item obj in Script.Info.Special)
            {
                if (obj.Vnum != -1)
                {
                    str2 += $"<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n";
                }
            }

            string str3 = str2 + $"</SpecialItems>\r\n" + $"<GiftItems>\r\n";
            foreach (Item bonu in Script.Info.Bonus)
            {
                if (bonu.Vnum != -1)
                {
                    str3 += $"<Item VNum=\"{bonu.Vnum}\" Amount=\"{bonu.Amount}\"/>\r\n";
                }
            }

            string str4 = str3 + $"</GiftItems>\r\n" + $"</Globals>\r\n" + $"<InstanceEvents>\r\n";
            foreach (Map map in Script.Maps.OrderBy(s => s.Id))
            {
                str4 += $"<CreateMap Map=\"{map.Id}\" VNum=\"{map.Vnum}\" IndexX=\"{map.IndexX}\" IndexY=\"{map.IndexY}\">\r\n";
                if (map.OnCharacterDiscoveringMap.Any())
                {
                    str4 += $"<OnCharacterDiscoveringMap>\r\n";
                    foreach (Event characterDiscovering in map.OnCharacterDiscoveringMap)
                    {
                        str4 += $"{characterDiscovering.SetEvent()}\r\n";
                    }

                    List<Event> characterDiscoveringMap = map.OnCharacterDiscoveringMap;
                    if (characterDiscoveringMap.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        str4 += $"<RefreshMapItems/>\r\n";
                    }

                    str4 += $"</OnCharacterDiscoveringMap>\r\n";
                }

                if (map.OnMapClear.Any())
                {
                    str4 += $"<OnMapClean>\r\n";
                    foreach (Event evt in map.OnMapClear)
                    {
                        str4 += $"{evt.SetEvent()}\r\n";
                    }

                    str4 += $"<RefreshMapItems/>\r\n";

                    str4 += $"</OnMapClean>\r\n";
                }

                if (map.Clock != null)
                {
                    str4 += $"<GenerateClock Value=\"{map.Clock.Time}\"/>\r\n";
                    str4 += $"<StartClock/>\r\n";
                }

                if (map.MapClock != null)
                {
                    str4 += $"<GenerateMapClock Value=\"{map.MapClock.Time}\"/>\r\n";
                }

                if (map.MapPortals.Any())
                {
                    str4 += $"\r\n<!-- Portals -->\r\n";
                    foreach (Portal portal in map.MapPortals)
                    {
                        if (portal.OnTraversalEvent.Any())
                        {
                            if (portal.DestMapId != 0)
                            {
                                str4 +=
                                    $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{(portal.PortalType == 4 ? 5 : portal.PortalType)}\" ToMap=\"{portal.DestMapId}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\">\r\n";
                            }
                            else
                            {
                                str4 +=
                                    $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{(portal.PortalType == 4 ? 5 : portal.PortalType)}\">\r\n";
                            }

                            str4 += $"<OnTraversal>\r\n";
                            foreach (Event evt in portal.OnTraversalEvent)
                            {
                                str4 += $"{evt.SetEvent()}\r\n";
                            }

                            List<Event> onTraversalEvent = portal.OnTraversalEvent;

                            if (portal.DestMapId == 0)
                            {
                                str4 += $"<End Type=\"5\"/>\r\n";
                            }

                            if (onTraversalEvent.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"<RefreshMapItems/>\r\n";
                            }

                            str4 += $"</OnTraversal>\r\n";
                            str4 += $"</SpawnPortal>\r\n";
                        }
                        else
                        {
                            str4 +=
                                $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{portal.PortalType}\" ToMap=\"{(portal.DestMapId == 0 ? LastPortal.DestMapId: portal.DestMapId)}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\"/>\r\n";
                        }
                    }
                }

                if (map.MapNpcs.Any())
                {
                    str4 += $"\r\n<!-- Npcs -->\r\n";
                    foreach (Npc npc in map.MapNpcs)
                    {
                        str4 +=
                            $"<SummonNpc VNum=\"{npc.Vnum}\" PositionX=\"{npc.PosX}\" PositionY=\"{npc.PosY}\" {(npc.IsMate ? "IsMate=\"True\"" : "")} {(npc.IsProtected ? "IsProtected=\"True\"" : "")}/>\r\n";
                    }
                }

                if (map.MapButtons.Any())
                {
                    int num = 0;
                    str4 += $"\r\n<!-- Buttons -->\r\n";
                    foreach (Button button in map.MapButtons)
                    {
                        if (button.OnFirstEnable.Any())
                        {
                            str4 +=
                                $"<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\">\r\n";
                            str4 += $"<OnFirstEnable>\r\n";
                            foreach (Event evt in button.OnFirstEnable)
                            {
                                str4 += $"{evt.SetEvent()}\r\n";
                            }

                            List<Event> onFirstEnable = button.OnFirstEnable;
                            if (onFirstEnable.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"<RefreshMapItems/>\r\n";
                            }

                            str4 += $"<RefreshMapItems/>\r\n";
                            str4 += $"</OnFirstEnable>\r\n";
                            str4 += $"</SpawnButton>\r\n";
                        }
                        else
                        {
                            str4 +=
                                $"<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\"/>\r\n";
                        }
                    }
                }

                if (map.MapMonsters.Any())
                {
                    str4 += $"\r\n<!-- Monsters -->\r\n";
                    foreach (Monster monster in map.MapMonsters)
                    {
                        if (!monster.OnDeathEvents.Any())
                        {
                            str4 +=
                                $"<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\" {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}/>\r\n";
                        }
                        else
                        {
                            str4 +=
                                $"<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\"  {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}>\r\n";
                            str4 += $"<OnDeath>\r\n";
                            foreach (Event deathEvent in monster.OnDeathEvents)
                            {
                                str4 += $"{deathEvent.SetEvent()}\r\n";
                            }

                            List<Event> deathEvents = monster.OnDeathEvents;
                            if (deathEvents.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                str4 += $"<RefreshMapItems/>\r\n";
                            }

                            str4 += $"</OnDeath>\r\n";
                            str4 += $"</SummonMonster>\r\n";
                        }
                    }
                }

                str4 += $"</CreateMap>\r\n";
            }

            string ret = XElement.Parse(str4 + $"</InstanceEvents>\r\n" + "</Definition>").ToString();
            return ret;
        }

        #endregion
    }
}