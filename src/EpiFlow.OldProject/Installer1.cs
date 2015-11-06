using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;
using System.Windows.Forms;


namespace EpiFlow
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        public bool RunInstallerAttribute
        {
            get { return true; }
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            try
            {
                RegistryKey root = Registry.ClassesRoot.OpenSubKey(@"WMP11.AssocFile.AVI\shell", true);

                RegistryKey keyL1 = root.OpenSubKey("Lookup Episode", true);
                if (keyL1 == null)
                    keyL1 = root.CreateSubKey("Lookup Episode");
                RegistryKey keyL2 = keyL1.OpenSubKey("command", true);
                if (keyL2 == null)
                    keyL2 = keyL1.CreateSubKey("command");
                keyL1.Close();
                keyL2.SetValue("", "\"%ProgramFiles(x86)%\\StuSoft\\EpiFlow\\EpiFlow.exe\" \"%1\"", RegistryValueKind.ExpandString);
                keyL2.Close();
                

                RegistryKey keyN1 = root.OpenSubKey("Name Episode", true);
                if (keyN1 == null)
                    keyN1 = root.CreateSubKey("Name Episode");
                RegistryKey keyN2 = keyN1.OpenSubKey("command", true);
                if (keyN2 == null)
                    keyN2 = keyN1.CreateSubKey("command");
                keyN1.Close();
                keyN2.SetValue("", "\"%ProgramFiles(x86)%\\StuSoft\\EpiFlow\\EpiFlow.exe\" \"%1\" 1", RegistryValueKind.ExpandString);
                keyN2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
            try
            {
                RegistryKey root = Registry.ClassesRoot.OpenSubKey(@"WMP11.AssocFile.AVI\shell", true);
                if (root.OpenSubKey("Lookup Episode") != null)
                    root.DeleteSubKeyTree("Lookup Episode");
                if (root.OpenSubKey("Name Episode") != null)
                    root.DeleteSubKeyTree("Name Episode");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            try
            {
                RegistryKey root = Registry.ClassesRoot.OpenSubKey(@"WMP11.AssocFile.AVI\shell", true);
                if (root.OpenSubKey("Lookup Episode") != null)
                    root.DeleteSubKeyTree("Lookup Episode");
                if (root.OpenSubKey("Name Episode") != null)
                    root.DeleteSubKeyTree("Name Episode");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
