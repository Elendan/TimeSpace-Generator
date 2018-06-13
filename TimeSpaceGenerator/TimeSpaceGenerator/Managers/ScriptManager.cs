using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //TODO: Split and cleanup this shit#endregion
        public string GenerateScript()
        {
            var script = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf - 8\"?>\r\n<Definition>\r\n" + $"<Globals>\r\n" + $"<Label Value=\"{Script.Info.Label}\"/>\r\n" +
                $"<Title Value=\"{Script.Info.Title}\"/>\r\n" + $"<LevelMinimum Value=\"{Script.Info.LevelMinimum}\"/>\r\n" +
                $"<LevelMaximum Value=\"{(Script.Info.LevelMax == 0 ? 99 : Script.Info.LevelMax)}\"/>\r\n" + $"<Lives Value=\"{Script.Info.Lives}\"/>\r\n" +
                $"<RequieredItems>\r\n" + $"<Item VNum=\"1012\" Amount=\"{Script.Info.SumOfRequired}\" />\r\n" + $"</RequieredItems>\r\n" + $"<DrawItems>\r\n");
            foreach (Item obj in Script.Info.DrawGift)
            {
                if (obj.Vnum != -1)
                {
                    script.Append($"<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n");
                }
            }

            script.Append($"</DrawItems>\r\n" + $"<SpecialItems>\r\n");
            foreach (Item obj in Script.Info.Special)
            {
                if (obj.Vnum != -1)
                {
                    script.Append($"<Item VNum=\"{obj.Vnum}\" Amount=\"{obj.Amount}\"/>\r\n");
                }
            }

            script.Append($"</SpecialItems>\r\n" + $"<GiftItems>\r\n");
            foreach (Item bonu in Script.Info.Bonus)
            {
                if (bonu.Vnum != -1)
                {
                    script.Append($"<Item VNum=\"{bonu.Vnum}\" Amount=\"{bonu.Amount}\"/>\r\n");
                }
            }

            script.Append($"</GiftItems>\r\n" + $"</Globals>\r\n" + $"<InstanceEvents>\r\n");
            foreach (Map map in Script.Maps.OrderBy(s => s.Id))
            {
                script.Append($"<CreateMap Map=\"{map.Id}\" VNum=\"{map.Vnum}\" IndexX=\"{map.IndexX}\" IndexY=\"{map.IndexY}\">\r\n");
                if (map.OnCharacterDiscoveringMap.Any())
                {
                    script.Append($"<OnCharacterDiscoveringMap>\r\n");
                    foreach (Event characterDiscovering in map.OnCharacterDiscoveringMap)
                    {
                        script.Append($"{characterDiscovering.SetEvent()}\r\n");
                    }

                    if (map.OnCharacterDiscoveringMap.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        script.Append($"<RefreshMapItems/>\r\n");
                    }

                    script.Append($"</OnCharacterDiscoveringMap>\r\n");
                }

                if (map.OnMapClear.Any())
                {
                    script.Append($"<OnMapClean>\r\n");
                    foreach (Event evt in map.OnMapClear)
                    {
                        script.Append($"{evt.SetEvent()}\r\n");
                    }

                    script.Append($"<RefreshMapItems/>\r\n");

                    script.Append($"</OnMapClean>\r\n");
                }

                if (map.Clock != null)
                {
                    script.Append($"<GenerateClock Value=\"{map.Clock.Time}\"/>\r\n");
                    script.Append($"<StartClock/>\r\n");
                }

                if (map.MapClock != null)
                {
                    script.Append($"<GenerateMapClock Value=\"{map.MapClock.Time}\"/>\r\n");
                }

                if (map.MapPortals.Any())
                {
                    script.Append($"\r\n<!-- Portals -->\r\n");
                    foreach (Portal portal in map.MapPortals)
                    {
                        if (portal.OnTraversalEvent.Any())
                        {
                            if (portal.DestMapId != 0)
                            {
                                script.Append(
                                    $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{(portal.PortalType == 4 ? 5 : portal.PortalType)}\" ToMap=\"{portal.DestMapId}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\">\r\n");
                            }
                            else
                            {
                                script.Append(
                                    $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{(portal.PortalType == 4 ? 5 : portal.PortalType)}\">\r\n");
                            }

                            script.Append($"<OnTraversal>\r\n");
                            foreach (Event evt in portal.OnTraversalEvent)
                            {
                                script.Append($"{evt.SetEvent()}\r\n");
                            }


                            if (portal.DestMapId == 0)
                            {
                                script.Append($"<End Type=\"5\"/>\r\n");
                            }

                            if (portal.OnTraversalEvent.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                script.Append($"<RefreshMapItems/>\r\n");
                            }

                            script.Append($"</OnTraversal>\r\n");
                            script.Append($"</SpawnPortal>\r\n");
                        }
                        else
                        {
                            script.Append(
                                $"<SpawnPortal IdOnMap=\"{portal.PortalId}\" PositionX=\"{portal.PosX}\" PositionY=\"{portal.PosY}\" Type=\"{portal.PortalType}\" ToMap=\"{(portal.DestMapId == 0 ? 1 : portal.DestMapId)}\" ToX =\"{portal.DestX}\" ToY =\"{portal.DestY}\"/>\r\n");
                        }
                    }
                }

                if (map.MapNpcs.Any())
                {
                    script.Append($"\r\n<!-- Npcs -->\r\n");
                    foreach (Npc npc in map.MapNpcs)
                    {
                        script.Append(
                            $"<SummonNpc VNum=\"{npc.Vnum}\" PositionX=\"{npc.PosX}\" PositionY=\"{npc.PosY}\" {(npc.IsMate ? "IsMate=\"True\"" : "")} {(npc.IsProtected ? "IsProtected=\"True\"" : "")}/>\r\n");
                    }
                }

                if (map.MapButtons.Any())
                {
                    int num = 0;
                    script.Append($"\r\n<!-- Buttons -->\r\n");
                    foreach (Button button in map.MapButtons)
                    {
                        if (button.OnFirstEnable.Any())
                        {
                            script.Append(
                                $"<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\">\r\n");
                            script.Append($"<OnFirstEnable>\r\n");
                            foreach (Event evt in button.OnFirstEnable)
                            {
                                script.Append($"{evt.SetEvent()}\r\n");
                            }

                            if (button.OnFirstEnable.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                script.Append($"<RefreshMapItems/>\r\n");
                            }

                            script.Append($"<RefreshMapItems/>\r\n");
                            script.Append($"</OnFirstEnable>\r\n");
                            script.Append($"</SpawnButton>\r\n");
                        }
                        else
                        {
                            script.Append(
                                $"<SpawnButton PositionX=\"{button.PosX}\" PositionY=\"{button.PosY}\" VNumDisabled=\"{button.DisableVNum}\" VNumEnabled=\"{button.EnableVNum}\" Id=\"{num++}\"/>\r\n");
                        }
                    }
                }

                if (map.MapMonsters.Any())
                {
                    script.Append($"\r\n<!-- Monsters -->\r\n");
                    foreach (Monster monster in map.MapMonsters)
                    {
                        if (!monster.OnDeathEvents.Any())
                        {
                            script.Append(
                                $"<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\" {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}/>\r\n");
                        }
                        else
                        {
                            script.Append(
                                $"<SummonMonster VNum=\"{monster.Vnum}\" PositionX=\"{monster.PosX}\" PositionY=\"{monster.PosY}\"  {(monster.IsTarget ? "IsTarget=\"True\"" : "")} {(monster.IsBonus ? "IsBonus=\"True\"" : "")}>\r\n");
                            script.Append($"<OnDeath>\r\n");
                            foreach (Event deathEvent in monster.OnDeathEvents)
                            {
                                script.Append($"{deathEvent.SetEvent()}\r\n");
                            }

                            if (monster.OnDeathEvents.Any(s => s.Type == EventType.ChangePortalType))
                            {
                                script.Append($"<RefreshMapItems/>\r\n");
                            }

                            script.Append($"</OnDeath>\r\n");
                            script.Append($"</SummonMonster>\r\n");
                        }
                    }
                }

                script.Append($"</CreateMap>\r\n");
            }

            return XElement.Parse(script.Append($"</InstanceEvents>\r\n</Definition>").ToString()).ToString();
        }

        #endregion Methods
    }
}