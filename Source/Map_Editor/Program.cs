using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NarcAPI;
using System.IO;

namespace WindowsFormsApplication1
{
    static class Program
    {
        public static string workingFolder;
        public static int gameID;
        public static bool isBW;
        public static bool isB2W2;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AppendPrivatePath("Data");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(ApplicationExit);
            Application.Run(new Form1());
        }

        public static void ApplicationExit(object sender, EventArgs e)
        {
            if (gameID == 0x45414441 || gameID == 0x45415041 || gameID == 0x53414441 || gameID == 0x53415041 || gameID == 0x46414441 || gameID == 0x46415041 || gameID == 0x49414441 || gameID == 0x49415041 || gameID == 0x44414441 || gameID == 0x44415041 || gameID == 0x4B414441 || gameID == 0x4B415041)
            {
                Narc.FromFolder(workingFolder + @"data\fielddata\mapmatrix\map_matrix\").Save(workingFolder + @"data\fielddata\mapmatrix\map_matrix.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\land_data\land_data_release").Save(workingFolder + @"data\fielddata\land_data\land_data_release.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\build_model\build_model").Save(workingFolder + @"data\fielddata\build_model\build_model.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set").Save(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset").Save(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset.narc");
                Narc.FromFolder(workingFolder + @"data\msgdata\msg\").Save(workingFolder + @"data\msgdata\msg.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\script\scr_seq_release").Save(workingFolder + @"data\fielddata\script\scr_seq_release.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\eventdata\zone_event_release").Save(workingFolder + @"data\fielddata\eventdata\zone_event_release.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_data").Save(workingFolder + @"data\fielddata\areadata\area_data.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\area_build").Save(workingFolder + @"data\fielddata\areadata\area_build_model\area_build.narc");
                Directory.Delete(workingFolder + @"data\fielddata\mapmatrix\map_matrix", true);
                Directory.Delete(workingFolder + @"data\fielddata\land_data\land_data_release", true);
                Directory.Delete(workingFolder + @"data\fielddata\build_model\build_model", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset", true);
                Directory.Delete(workingFolder + @"data\msgdata\msg", true);
                Directory.Delete(workingFolder + @"data\fielddata\script\scr_seq_release", true);
                Directory.Delete(workingFolder + @"data\fielddata\eventdata\zone_event_release", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_data", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\area_build", true);
            }
            if (gameID == 0x4A414441 || gameID == 0x4A415041)
            {
                Narc.FromFolder(workingFolder + @"data\fielddata\mapmatrix\map_matrix\").Save(workingFolder + @"data\fielddata\mapmatrix\map_matrix.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\build_model\build_model").Save(workingFolder + @"data\fielddata\build_model\build_model.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set").Save(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset").Save(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset.narc");
                Narc.FromFolder(workingFolder + @"data\msgdata\msg\").Save(workingFolder + @"data\msgdata\msg.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\land_data\land_data").Save(workingFolder + @"data\fielddata\land_data\land_data.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\script\scr_seq").Save(workingFolder + @"data\fielddata\script\scr_seq.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\eventdata\zone_event").Save(workingFolder + @"data\fielddata\eventdata\zone_event.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_data").Save(workingFolder + @"data\fielddata\areadata\area_data.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\area_build").Save(workingFolder + @"data\fielddata\areadata\area_build_model\area_build.narc");
                Directory.Delete(workingFolder + @"data\fielddata\mapmatrix\map_matrix\", true);
                Directory.Delete(workingFolder + @"data\fielddata\build_model\build_model", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset", true);
                Directory.Delete(workingFolder + @"data\msgdata\msg", true);
                Directory.Delete(workingFolder + @"data\fielddata\land_data\land_data", true);
                Directory.Delete(workingFolder + @"data\fielddata\script\scr_seq", true);
                Directory.Delete(workingFolder + @"data\fielddata\eventdata\zone_event", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_data", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\area_build", true);
            }

            if (gameID == 0x45555043 || gameID == 0x53555043 || gameID == 0x46555043 || gameID == 0x49555043 || gameID == 0x44555043 || gameID == 0x4A555043 || gameID == 0x4B555043)
            {
                Narc.FromFolder(workingFolder + @"data\fielddata\mapmatrix\map_matrix\").Save(workingFolder + @"data\fielddata\mapmatrix\map_matrix.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\land_data\land_data").Save(workingFolder + @"data\fielddata\land_data\land_data.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\build_model\build_model").Save(workingFolder + @"data\fielddata\build_model\build_model.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset").Save(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set").Save(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set.narc");
                Narc.FromFolder(workingFolder + @"data\msgdata\pl_msg\").Save(workingFolder + @"data\msgdata\pl_msg.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\script\scr_seq").Save(workingFolder + @"data\fielddata\script\scr_seq.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\eventdata\zone_event").Save(workingFolder + @"data\fielddata\eventdata\zone_event.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_data").Save(workingFolder + @"data\fielddata\areadata\area_data.narc");
                Narc.FromFolder(workingFolder + @"data\fielddata\areadata\area_build_model\area_build").Save(workingFolder + @"data\fielddata\areadata\area_build_model\area_build.narc");
                Directory.Delete(workingFolder + @"data\fielddata\mapmatrix\map_matrix\", true);
                Directory.Delete(workingFolder + @"data\fielddata\land_data\land_data", true);
                Directory.Delete(workingFolder + @"data\fielddata\build_model\build_model", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_map_tex\map_tex_set", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset", true);
                Directory.Delete(workingFolder + @"data\msgdata\pl_msg", true);
                Directory.Delete(workingFolder + @"data\fielddata\script\scr_seq", true);
                Directory.Delete(workingFolder + @"data\fielddata\eventdata\zone_event", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_data", true);
                Directory.Delete(workingFolder + @"data\fielddata\areadata\area_build_model\area_build", true);
            }
            if (gameID == 0x454B5049 || gameID == 0x45475049 || gameID == 0x534B5049 || gameID == 0x53475049 || gameID == 0x464B5049 || gameID == 0x46475049 || gameID == 0x494B5049 || gameID == 0x49475049 || gameID == 0x444B5049 || gameID == 0x44475049 || gameID == 0x4A4B5049 || gameID == 0x4A475049 || gameID == 0x4B4B5049 || gameID == 0x4B475049)
            {
                Narc.FromFolder(workingFolder + @"data\a\0\4\matrix\").Save(workingFolder + @"data\a\0\4\1");
                Narc.FromFolder(workingFolder + @"data\a\0\6\map\").Save(workingFolder + @"data\a\0\6\5");
                Narc.FromFolder(workingFolder + @"data\a\0\4\building\").Save(workingFolder + @"data\a\0\4\0");
                Narc.FromFolder(workingFolder + @"data\a\0\4\texture\").Save(workingFolder + @"data\a\0\4\4");
                Narc.FromFolder(workingFolder + @"data\a\0\7\textureBld\").Save(workingFolder + @"data\a\0\7\0");
                Narc.FromFolder(workingFolder + @"data\a\0\2\text\").Save(workingFolder + @"data\a\0\2\7");
                Narc.FromFolder(workingFolder + @"data\a\0\1\script\").Save(workingFolder + @"data\a\0\1\2");
                Narc.FromFolder(workingFolder + @"data\a\0\3\event\").Save(workingFolder + @"data\a\0\3\2");
                Directory.Delete(workingFolder + @"data\a\0\4\matrix\", true);
                Directory.Delete(workingFolder + @"data\a\0\6\map\", true);
                Directory.Delete(workingFolder + @"data\a\0\4\building\", true);
                Directory.Delete(workingFolder + @"data\a\0\4\texture\", true);
                Directory.Delete(workingFolder + @"data\a\0\7\textureBld\", true);
                Directory.Delete(workingFolder + @"data\a\0\2\text\", true);
                Directory.Delete(workingFolder + @"data\a\0\1\script\", true);
                Directory.Delete(workingFolder + @"data\a\0\3\event\", true);
            }
            if (isBW == true)
            {
                Narc.FromFolder(workingFolder + @"data\a\0\0\maps\").Save(workingFolder + @"data\a\0\0\8");
                Narc.FromFolder(workingFolder + @"data\a\0\1\headers\").Save(workingFolder + @"data\a\0\1\2");
                Narc.FromFolder(workingFolder + @"data\a\0\0\matrix\").Save(workingFolder + @"data\a\0\0\9");
                Narc.FromFolder(workingFolder + @"data\a\0\1\tilesets\").Save(workingFolder + @"data\a\0\1\4");
                Narc.FromFolder(workingFolder + @"data\a\1\7\bldtilesets").Save(workingFolder + @"data\a\1\7\6");
                Narc.FromFolder(workingFolder + @"data\a\1\7\bld2tilesets").Save(workingFolder + @"data\a\1\7\7");
                Narc.FromFolder(workingFolder + @"data\a\0\0\texts\").Save(workingFolder + @"data\a\0\0\2");
                Narc.FromFolder(workingFolder + @"data\a\0\0\texts2\").Save(workingFolder + @"data\a\0\0\3");
                Narc.FromFolder(workingFolder + @"data\a\0\5\scripts\").Save(workingFolder + @"data\a\0\5\7");
                Directory.Delete(workingFolder + @"data\a\0\0\maps\", true);
                Directory.Delete(workingFolder + @"data\a\0\1\headers\", true);
                Directory.Delete(workingFolder + @"data\a\0\0\matrix\", true);
                Directory.Delete(workingFolder + @"data\a\0\1\tilesets\", true);
                Directory.Delete(workingFolder + @"data\a\1\7\bldtilesets", true);
                Directory.Delete(workingFolder + @"data\a\1\7\bld2tilesets", true);
                Directory.Delete(workingFolder + @"data\a\0\0\texts\", true);
                Directory.Delete(workingFolder + @"data\a\0\0\texts2\", true);
                Directory.Delete(workingFolder + @"data\a\0\5\scripts\", true);
            }
            if (isB2W2 == true)
            {
                Narc.FromFolder(workingFolder + @"data\a\0\0\maps\").Save(workingFolder + @"data\a\0\0\8");
                Narc.FromFolder(workingFolder + @"data\a\0\1\headers\").Save(workingFolder + @"data\a\0\1\2");
                Narc.FromFolder(workingFolder + @"data\a\0\0\matrix\").Save(workingFolder + @"data\a\0\0\9");
                Narc.FromFolder(workingFolder + @"data\a\0\1\tilesets\").Save(workingFolder + @"data\a\0\1\4");
                Narc.FromFolder(workingFolder + @"data\a\1\7\bldtilesets").Save(workingFolder + @"data\a\1\7\4");
                Narc.FromFolder(workingFolder + @"data\a\1\7\bld2tilesets").Save(workingFolder + @"data\a\1\7\5");
                Narc.FromFolder(workingFolder + @"data\a\0\0\texts\").Save(workingFolder + @"data\a\0\0\2");
                Narc.FromFolder(workingFolder + @"data\a\0\0\texts2\").Save(workingFolder + @"data\a\0\0\3");
                Narc.FromFolder(workingFolder + @"data\a\0\5\scripts\").Save(workingFolder + @"data\a\0\5\6");
                Directory.Delete(workingFolder + @"data\a\0\0\maps\", true);
                Directory.Delete(workingFolder + @"data\a\0\1\headers\", true);
                Directory.Delete(workingFolder + @"data\a\0\0\matrix\", true);
                Directory.Delete(workingFolder + @"data\a\0\1\tilesets\", true);
                Directory.Delete(workingFolder + @"data\a\1\7\bldtilesets", true);
                Directory.Delete(workingFolder + @"data\a\1\7\bld2tilesets", true);
                Directory.Delete(workingFolder + @"data\a\0\0\texts\", true);
                Directory.Delete(workingFolder + @"data\a\0\0\texts2\", true);
                Directory.Delete(workingFolder + @"data\a\0\5\scripts\", true);
            }
        }
    }
}
