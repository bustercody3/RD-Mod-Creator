using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace RecklessDriverModCreator
{
    public partial class Form1 : Form
    {
        int gearNums1;
        public Form1()
        {
            InitializeComponent();
        }

        private void objButton_Click(object sender, EventArgs e)
        {
            //Opens a file selection dialog, and only shows OBJ files
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "obj files (*.obj)|*.obj" })
            {
                ofd.InitialDirectory = Directory.GetCurrentDirectory();  //The folder you initially see is set to the folder which contains the EXE
                ofd.Title = "Browse OBJ Files";
                ofd.CheckFileExists = true;  //If the path in the text box of the file dialog does not exist it warns the user
                ofd.CheckPathExists = true;  //If the folder  in the text box of the file dialog does not exist it warns the user

                //Runs if you press the Okay button to confirm the file, or double click a file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    objLabel.Text = ofd.FileName;  //Shows what file was selected by changing the text of the label beside the OBJ button
                }
            }
        }

        //Runs when I click the Generate Mod button
        private void genModButton_Click(object sender, EventArgs e) 
        {
            //Gets the directory the EXE is in, then the subfolders mod/vehicles/models/vpCarName and sets the variable folder to this
            var folder = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp"+carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds"));

            //if this directory doesn't exist, it creates it
            if (!folder.Exists)
            {
                folder.Create();
            }

            //Copies the OBJ file that was selected into the mod/vehicles/models/vpCarName folder
            File.Copy(objLabel.Text, Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp"+carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".obj"), true);

            //Copies the MTL file that was selected into the mod/vehicles/models/vpCarName folder
            File.Copy(mtlLabel.Text, Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".mtl"), true);

            //Copies the default idle sound if nothing is selected by the user
            if (idleLabel.Text == "")
            {
                File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Sounds", "vpGlad_Idle.wav"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Idle.wav"), true);
            }
            //Copies selected idle sound
            else
            {
                File.Copy(idleLabel.Text, Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Idle.wav"), true);
            }

            if (gasLabel.Text == "")
            {
                File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Sounds", "vpGlad_Mid.wav"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Mid.wav"), true);
            }
            //Copies selected idle sound
            else
            {
                File.Copy(idleLabel.Text, Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Idle.wav"), true);
            }

            //Copies the default horn sound if nothing is selected by the user
            if (hornLabel.Text == "")
            {
                File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Sounds", "vpGlad_Horn.wav"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Horn.wav"), true);
            }
            //Copies selected horn sound
            else
            {
                File.Copy(hornLabel.Text, Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Horn.wav"), true);
            }
            copyTunes();
            //Gets obj path and edits the 3rd line of the obj to "mttlib vpCarName.mtl so that the mtl file is attached to the obj
            var theOBJ = Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".obj");
            lineChanger("mtllib "+ "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" "))+".mtl", theOBJ, 3);
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "vpSuperfly.dmon"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "sounds", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Idle.wav"), true);
            checkGears();
            
            mainDmon(carDescText.Text, paintText.Text);
            opp(gearNums1.ToString(), speedText.Text, steerText.Text, torqueText.Text);

            setTunes();
        }
        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        private void mtlButton_Click(object sender, EventArgs e)
        {
            //Opens a file selection dialog, and only shows MTL files
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "mtl files (*.mtl)|*.mtl" })
            {
                ofd.InitialDirectory = Directory.GetCurrentDirectory();  //The folder you initially see is set to the folder which contains the EXE
                ofd.Title = "Browse MTL Files";
                ofd.CheckFileExists = true;  //If the path in the text box of the file dialog does not exist it warns the user
                ofd.CheckPathExists = true;  //If the folder  in the text box of the file dialog does not exist it warns the user

                //Runs if you press the Okay button to confirm the file, or double click a file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    mtlLabel.Text = ofd.FileName;  //Shows what file was selected by changing the text of the label beside the MTL button
                }
            }
        }

        private void idleButton_Click(object sender, EventArgs e)
        {
            //Opens a file selection dialog, and only shows wav files
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "wav files (*.wav)|*.wav" })
            {
                ofd.InitialDirectory = Directory.GetCurrentDirectory();  //The folder you initially see is set to the folder which contains the EXE
                ofd.Title = "Browse WAV Files";
                ofd.CheckFileExists = true;  //If the path in the text box of the file dialog does not exist it warns the user
                ofd.CheckPathExists = true;  //If the folder  in the text box of the file dialog does not exist it warns the user

                //Runs if you press the Okay button to confirm the file, or double click a file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    idleLabel.Text = ofd.FileName;  //Shows what file was selected by changing the text of the label beside the MTL button
                }
            }
        }

        private void hornButton_Click(object sender, EventArgs e)
        {
            //Opens a file selection dialog, and only shows wav files
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "wav files (*.wav)|*.wav" })
            {
                ofd.InitialDirectory = Directory.GetCurrentDirectory();  //The folder you initially see is set to the folder which contains the EXE
                ofd.Title = "Browse WAV Files";
                ofd.CheckFileExists = true;  //If the path in the text box of the file dialog does not exist it warns the user
                ofd.CheckPathExists = true;  //If the folder  in the text box of the file dialog does not exist it warns the user

                //Runs if you press the Okay button to confirm the file, or double click a file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    hornLabel.Text = ofd.FileName;  //Shows what file was selected by changing the text of the label beside the MTL button
                }
            }
        }

        private void mainDmon(string showcase, string colours)
        {
            //Copy main dmon file for vehicle and renames it to vpCarName.dmon
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "vpSuperfly.dmon"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".dmon"), true);
            var theDMON = Path.Combine(Directory.GetCurrentDirectory(), "Default", "vpSuperfly.dmon");

            lineChanger("baseName = " + "\"" + "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "\"", theDMON, 1);
            lineChanger("vehicleName = " + '\u0022' + carNameBox.Text + '\u0022', theDMON, 2);
            lineChanger("colors = " + '\u0022' + colours + '\u0022', theDMON, 3);
            lineChanger("showcaseDescription = " + '\u0022' + showcase + '\u0022', theDMON, 5);
            /*
             baseName = "vpCarName"
             vehicleName = "Car Name"
             colors = ["Colour1, Colour2"]
             carLocked = false
             showcaseDescription = "Light, fast, but durability isn't a strongsuit"
             uiScale = 1
             */
        }

        private void opp(string gearNums, string topSpeed, string maxSteer, string maxTorque)
        {
            var theOPP = Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_opp.tune");  //Sets variable of the OPP file for easy access
            lineChanger("maxSteer = " + maxSteer, theOPP, 1);
            lineChanger("maxTorque = " + maxTorque, theOPP, 2);
            lineChanger("topSpeed = " + topSpeed, theOPP, 3);
            lineChanger("gearNumber = " + gearNums, theOPP, 4);
            

            //Set the gears to what the textboxes say
            if(gearNums == "0") { lineChanger("gear0 = " + gearText_0.Text, theOPP, 5); }
            if (gearNums == "1") { lineChanger("gear1 = " + gearText_1.Text, theOPP, 6); }
            if (gearNums == "2") { lineChanger("gear2 = " + gearText_2.Text, theOPP, 7); }
            if (gearNums == "3") { lineChanger("gear3 = " + gearText_3.Text, theOPP, 8); }
            if (gearNums == "4") { lineChanger("gear4 = " + gearText_4.Text, theOPP, 9); }
            if (gearNums == "5") { lineChanger("gear5 = " + gearText_5.Text, theOPP, 10); }
            if (gearNums == "6") { lineChanger("gear6 = " + gearText_6.Text, theOPP, 11); }
            if (gearNums == "7") { lineChanger("gear7 = " + gearText_7.Text, theOPP, 12); }
            if (gearNums == "8") { lineChanger("gear8 = " + gearText_8.Text, theOPP, 13); }
            if (gearNums == "9") { lineChanger("gear9 = " + gearText_9.Text, theOPP, 14); }
            if (gearNums == "10") { lineChanger("gear10 = " + gearText_10.Text, theOPP, 15); }
        }

        private void copyTunes()
        {
            //Copies main tune and renames it to vpCarName.tune
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Tunes", "vpSuperfly.tune"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".tune"), true);

            //Copies opp tune and renames it to vpCarName_opp.tune
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Tunes", "vpSuperfly_opp.tune"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_opp.tune"), true);

            //Copies cam tune and renames it to vpCarName_Cam.tune
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "Default", "Tunes", "vpSuperfly_Cam.tune"), Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + "_Cam.tune"), true);
        }

        //Sets the lines to a specified value in the tune file
        private void setTunes()
        {
            var theTune = Path.Combine(Directory.GetCurrentDirectory(), carNameBox.Text + " Vehicle Mod", "vehicles", "models", "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")), "vp" + carNameBox.Text.Substring(0, carNameBox.Text.IndexOf(" ")) + ".tune"); //Sets variable of the tune file for easy access

            //Changes the default setting of turbo from true to false in the tune
            if (turboCheck.Checked == false)
            {
                lineChanger("  turbo = false", theTune, 27);
            }

            lineChanger("  maxPower = " + maxPowerText.Text, theTune, 2);
            lineChanger("  rigidBodyDrag = " + dragText.Text, theTune, 6);
            lineChanger("  rigidBodyAngularDrag = " + angularDragText.Text, theTune, 7);
            lineChanger("  rigidBodyMass = " + massText.Text, theTune, 8);
            lineChanger("  COMHeight = " + vCOMText.Text, theTune, 9);
            lineChanger("  COMZPos = " + hCOMText.Text, theTune, 10);
            lineChanger("  minRPM = " + minRPMText.Text, theTune, 11);
            lineChanger("  maxRPM = " + maxRPMText.Text, theTune, 12);
            lineChanger("  maxDamage = " + maxDamageText.Text, theTune, 28);
            lineChanger("  stabilityControl = " + stabilityControlText.Text, theTune, 32);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            /* var numVal = numericUpDown1.Value;

            int count = 2;
            int y = 48;

            for (int i = 0; i < numVal; i++)
            {
                //Create the dynamic TextBox.
                TextBox text1 = new TextBox();
                text1.Location = new Point(39, y);
                text1.Name = "gearText_" + count;
                count += 1;
                panel1.Controls.Add(text1); //Makes the textbox visible
                y += 30; //Makes next textbox 30 y below the previous
            }
            */
            
        }

        private void gearText_1_TextChanged(object sender, EventArgs e)
        {

        }

        private void gearText_8_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkGears()
        {
            if (gearText_1.Text != "") gearNums1 = 1;
            if (gearText_2.Text != "") gearNums1 = 2;
            if (gearText_3.Text != "") gearNums1 = 3;
            if (gearText_4.Text != "") gearNums1 = 4;
            if (gearText_5.Text != "") gearNums1 = 5;
            if (gearText_6.Text != "") gearNums1 = 6;
            if (gearText_7.Text != "")  gearNums1 = 7;
            if (gearText_8.Text != "")  gearNums1 = 8;
            if (gearText_9.Text != "") gearNums1 = 9;
            if (gearText_10.Text != "") gearNums1 = 10;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hornLabel_Click(object sender, EventArgs e)
        {

        }

        private void llabel21_Click(object sender, EventArgs e)
        {

        }
    }
}
