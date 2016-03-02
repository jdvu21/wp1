using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO; // for file browsing
using System.Reflection;

class MyForm : Form
{
    // string to be changed by the user at run time
    string dllFileName = "sample.txt";
    bool dllLoaded = false;
    string[] classList;
    int classCount = 0; // to be passed when the form needs to be remade

    MyForm()
    {
        // title text
        Text = "Calculator GUI";

        // sets the windows size?
        ClientSize = new Size(800, 600);

        // menu creation
        MainMenu menu = new MainMenu();
        MenuItem item = menu.MenuItems.Add("&File");
        item.MenuItems.Add("Open...", new EventHandler(OnOpen));
        item.MenuItems.Add("-"); // minimize button?
        item.MenuItems.Add("E&xit", new EventHandler(OnExit));

        //attach the menu to the form
        Menu = menu;
    }

    void OnOpen(object sender, EventArgs e)
    {
        // create the dialog 
        MyDialog dlg = new MyDialog();

        // initialize the input box with a sample string
        dlg.UserFile = "Math.dll";

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            // If the dialog was dismissed with the OK button,
            // extract the user input and open the .dll file
            dllFileName = dlg.UserFile;
            dllLoaded = true; // set the flag to change the form

            #region Reflection Stuff
            Assembly a = Assembly.LoadFrom(dllFileName);
            Type type = a.GetType(dllFileName);
            Console.WriteLine(a);
            Console.WriteLine(type);
            
            foreach(Type className in a.GetExportedTypes())
            {
                // we use a dynamic object to get the classes inside the .dll
                dynamic c = Activator.CreateInstance(className);
                Console.WriteLine(c);
                classCount++;
                classList[classCount] = c.ToString();

                // we're looking for these exact methods
                // maybe later on, look into dynamically listing all methods
                Console.WriteLine(className.GetMethod("addition"));
                Console.WriteLine(className.GetMethod("subtraction"));
                Console.WriteLine(className.GetMethod("multiplication"));
                Console.WriteLine(className.GetMethod("division"));
                //Console.WriteLine(myMethodInfo);
            }
            #endregion

            Invalidate();
            CalcForm calc = new CalcForm();
            //Invalidate();
        }

    }

    void OnExit(object sender, EventArgs e)
    {
        Close();
    }


    //the main function... all the way down here
    static void Main()
    {
        Application.Run(new MyForm());
    }
}

class MyDialog : Form
{
    Label dllLabel;
    TextBox dllBox;
    Button OKButton;
    Button NotOKButton;

    public string UserFile
    {
        get { return dllBox.Text; }
        set { dllBox.Text = value.ToString(); }
    }

    public MyDialog()
    {
        ClientSize = new Size(296, 196);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Text = "Open a .dll";
        ShowInTaskbar = false;

        dllLabel = new Label();
        dllLabel.Location = new Point(16, 16);
        dllLabel.Size = new Size(120, 30);
        dllLabel.Text = "Name of target file:";

        dllBox = new TextBox();
        dllBox.Location = new Point(50, 50);
        dllBox.Size = new Size(96, 24);
        dllBox.TabIndex = 1;

        OKButton = new Button();
        OKButton.Location = new Point(184, 12);
        OKButton.Size = new Size(96, 24);
        OKButton.TabIndex = 3;
        OKButton.Text = "OK";
        OKButton.DialogResult = DialogResult.OK;

        NotOKButton = new Button();
        NotOKButton.Location = new Point(184, 44);
        NotOKButton.Size = new Size(96, 24);
        NotOKButton.TabIndex = 4;
        NotOKButton.Text = "Cancel";
        NotOKButton.DialogResult = DialogResult.Cancel;

        AcceptButton = OKButton;
        CancelButton = NotOKButton;

        Controls.Add(OKButton);
        Controls.Add(NotOKButton);
        Controls.Add(dllLabel);
        Controls.Add(dllBox);
    }
}

class CalcForm : Form
{
    Label classLabel1, classLabel2;
    Label function1; 
    Label function2; 
    Label function3;
    Label function4;
    Label function5;
    Label function6;
    Label function7;
    Label function8;

    Button addButton, subButton, multiButton, divideButton, buildButton;
    
    public CalcForm()
    {
        ClientSize = new Size(800, 600);

        classLabel1 = new Label();
        classLabel1.Location = new Point(16, 16);
        classLabel1.Size = new Size(120, 30);
        classLabel1.Text = "Simple";

    }
}