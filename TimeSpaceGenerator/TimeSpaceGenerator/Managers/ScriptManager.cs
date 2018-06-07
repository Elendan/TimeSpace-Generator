using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Script = new Objects.Script();
            MapMonsters = new List<Monster>();
            Data = new object[5];
        }

        #endregion

        #region Properties

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

        public byte IndexX { get; set; }

        public byte IndexY { get; set; }

        public bool Flag1 { get; set; }

        public bool Flag2 { get; set; }

        public bool Flag3 { get; set; }

        public object Target { get; set; }

        public string EventName { get; set; }

        public object[] Data { get; set; }

        #endregion

        #region Methods

        public void AddEvent(object target, string eventName, Event evt)
        {
            if (target is Monster monster)
            {
                monster.OnDeathEvents.Add(evt);
            }
            else
            {
                Map map;
                if ((map = target as Map) != null)
                {
                    string str = eventName;
                    if (str != "OnCharacterDiscoveringMap")
                    {
                        if (str != "OnMapClear")
                        {
                            return;
                        }

                        map.OnMapClear.Add(evt);
                    }
                    else
                    {
                        map.OnCharacterDiscoveringMap.Add(evt);
                    }
                }
                else
                {
                    if (target is Button button)
                    {
                        button.OnFirstEnable.Add(evt);
                    }
                    else
                    {
                        if (!(target is Portal portal))
                        {
                            return;
                        }

                        portal.OnTraversalEvent.Add(evt);
                    }
                }
            }
        }

        #endregion
    }
}
