using System;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using System.Text;
using AresAtWar.Init;
using AresAtWar.War;
using System.Linq;

namespace Digi.Examples
{
    // Text Surface Scripts (TSS) can be selected in any LCD's scripts list.
    // These are meant as fast no-sync (sprites are not sent over network) display scripts, and the Run() method only executes player-side (no DS).
    // You can still use a session comp and access it through this to use for caches/shared data/etc.
    //
    // The display name has localization support aswell, same as a block's DisplayName in SBC.
    [MyTextSurfaceScript("InternalNameHere", "AaW FrontLine Tracker")]
    public class SomeClassName : MyTSSCommon
    {

        public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10; // frequency that Run() is called.

        private readonly IMyTerminalBlock TerminalBlock;

        public SomeClassName(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
        {
            TerminalBlock = (IMyTerminalBlock)block; // internal stored m_block is the ingame interface which has no events, so can't unhook later on, therefore this field is required.
            TerminalBlock.OnMarkForClose += BlockMarkedForClose; // required if you're gonna make use of Dispose() as it won't get called when block is removed or grid is cut/unloaded.

            // Called when script is created.
            // This class is instanced per LCD that uses it, which means the same block can have multiple instances of this script aswell (e.g. a cockpit with all its screens set to use this script).
        }

        public override void Dispose()
        {
            base.Dispose(); // do not remove
            TerminalBlock.OnMarkForClose -= BlockMarkedForClose;

            // Called when script is removed for any reason, so that you can clean up stuff if you need to.
        }

        void BlockMarkedForClose(IMyEntity ent)
        {
            Dispose();
        }

        // gets called at the rate specified by NeedsUpdate
        // it can't run every tick because the LCD is capped at 6fps anyway.
        public override void Run()
        {
            try
            {
                base.Run(); // do not remove



                Draw();
            }
            catch (Exception e) // no reason to crash the entire game just for an LCD script, but do NOT ignore them either, nag user so they report it :}
            {
                DrawError(e);
            }
        }

        void Draw() // this is a custom method which is called in Run().
        {
            Vector2 screenSize = Surface.SurfaceSize;
            Vector2 screenCorner = (Surface.TextureSize - screenSize) * 0.5f;

            var frame = Surface.DrawFrame();
            Vector2 padding = Surface.TextureSize * (Surface.TextPadding / 100);
            var viewport = new RectangleF((Surface.TextureSize - Surface.SurfaceSize) / 2f + padding, Surface.SurfaceSize - (2 * padding));

            // Drawing sprites works exactly like in PB API.
            // Therefore this guide applies: https://github.com/malware-dev/MDK-SE/wiki/Text-Panels-and-Drawing-Sprites

            // there are also some helper methods from the MyTSSCommon that this extends.
            // like: AddBackground(frame, Surface.ScriptBackgroundColor); - a grid-textured background

            var sb = new StringBuilder();
            sb.Append("ActiveFrontLines");
            sb.AppendLine();

            for (int i = 0; i < WarSim._frontlines.Count; i++)
            {
                var frontline = WarSim._frontlines[i];

                var nodeA = WarSim._nodes.FirstOrDefault(n => n.Id == frontline.NodeAId);
                var nodeB = WarSim._nodes.FirstOrDefault(n => n.Id == frontline.NodeBId);

                if (frontline.Score != 0)
                {

                    // First line: "GC;  -10 ;FAF" (score is always 6 characters wide)
                    string formattedScore = FormatScore(frontline.Score); 
                    sb.Append($"{Format(nodeA.Faction.Tag)};{formattedScore};{Format(nodeB.Faction.Tag, false)}");
                    sb.AppendLine();

                    // Calculate padding for second line to center the names
                    int paddingLength = Math.Abs(nodeA.FancyName.Length - nodeB.FancyName.Length);
                    string apadding = new string(' ', paddingLength);

                    if (nodeA.FancyName.Length > nodeB.FancyName.Length)
                    {
                        sb.AppendFormat("{0};     ;{2}{1}", nodeA.FancyName, apadding, nodeB.FancyName);
                    }
                    else
                    {
                        sb.AppendFormat("{1}{0};     ;{2}", nodeA.FancyName, apadding, nodeB.FancyName);
                    }
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }

            var textTTS = MySprite.CreateText(sb.ToString(), "Monospace", Surface.ScriptForegroundColor, 0.6f, TextAlignment.CENTER);
            textTTS.Position = screenCorner + new Vector2(screenSize.X / 2, 8);
            frame.Add(textTTS);

            // add more sprites and stuff

            frame.Dispose(); // send sprites to the screen
        }

        string FormatScore(int score)
        {
            string scoreString = score.ToString();
            switch (scoreString.Length)
            {
                case 1:
                    return $"  {scoreString}  "; // e.g. "  1  "
                case 2:
                    if (score>0)
                    {
                        return $"  {scoreString} ";  // e.g. " 10 "
                    }
                    return $" {scoreString}  ";  // e.g. " -1 "


                case 3:
                    if (score > 0)
                    {
                        return $"  {scoreString}";  // e.g. " 100 "
                    }
                    return $"{scoreString}  ";   // e.g. "-10""
                case 4:
                    return $"{scoreString} ";    // e.g. "-100"
                default:
                    return scoreString;          // If the score is somehow longer than expected
            }
        }

        void DrawError(Exception e)
        {
            MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

            try // first try printing the error on the LCD
            {
                Vector2 screenSize = Surface.SurfaceSize;
                Vector2 screenCorner = (Surface.TextureSize - screenSize) * 0.5f;

                var frame = Surface.DrawFrame();

                var bg = new MySprite(SpriteType.TEXTURE, "SquareSimple", null, null, Color.Black);
                frame.Add(bg);

                var text = MySprite.CreateText($"ERROR: {e.Message}\n{e.StackTrace}\n\nPlease send screenshot of this to mod author.\n{MyAPIGateway.Utilities.GamePaths.ModScopeName}", "White", Color.Red, 0.7f, TextAlignment.LEFT);
                text.Position = screenCorner + new Vector2(16, 16);
                frame.Add(text);

                frame.Dispose();
            }
            catch (Exception e2)
            {
                MyLog.Default.WriteLineAndConsole($"Also failed to draw error on screen: {e2.Message}\n{e2.StackTrace}");

                if (MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
            }
        }



        public static string Format(string input, bool left = true)
        {
            //5
            if (input.Length == 5)
                return input;


            if (input.Length == 4)
                if(left)
                    return " "+input;
                else
                    return input + " ";

            if (input.Length == 3)
                if (left)
                    return "  " + input;
                else
                    return input + "  ";

            if (input.Length == 2)
                if (left)
                    return "   " + input;
                else
                    return input + "   ";


            if (input.Length == 1)
                if (left)
                    return "    " + input;
                else
                    return input + "    ";

            return "Err";

        }

    }


}