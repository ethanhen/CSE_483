using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using MyIntegerSet;

namespace HW1
{
    class Model : INotifyPropertyChanged
    {

        // string arrays for split()
        string[] set1String = null;
        string[] set2String = null;

        // lists because lists > arrays
        List<int> set1 = new List<int>();
        List<int> set2 = new List<int>();

        // sets for set operations
        IntegerSet s1 = new IntegerSet();
        IntegerSet s2 = new IntegerSet();

        bool errorFlag = false;

        int temp;

        // called on update click
        public void Update()
        {
            // clear any error messages in the status box
            StatusBox = "";

            // check if either input is null or empty
            if (Input1Box == null || Input2Box == null || Input1Box == "" || Input2Box == "")
            {
                // display error message and clear outputs
                inputError("You cannot enter empty sets!");
            }
            else
            {
                // use the split operation to put data into strings
                // using string arrays as there can be non-integer
                // inputs which we will deal with later.
                set1String = Input1Box.Split(',');
                set2String = Input2Box.Split(',');

                // iterate through each item in the string array that we created
                foreach(string x in set1String)
                {
                    // check to make sure the array only has integers
                    if (!int.TryParse(x, out temp))
                    {
                        // display error message and clear outputs
                        errorFlag = true;
                        break;
                    }
                    else
                    {
                        // otherwise add to list of integers
                        set1.Add(temp);
                    }
                }

                // iterate through other string array, same as other loop
                foreach (string x in set2String)
                {
                    if (!int.TryParse(x, out temp))
                    {
                        errorFlag = true;
                        break;
                    }
                    else
                    {
                        set2.Add(temp);
                    }
                }

                // if there was no error, compute the sets
                // otherwise display error message
                if (!errorFlag)
                {
                    // transfer elements from lists to integerset objects
                    foreach (int x in set1)
                    {
                        s1.InsertElement(x);
                    }

                    foreach (int x in set2)
                    {
                        s2.InsertElement(x);
                    }

                    // calculate union and intersection
                    IntegerSet unionSet = s1.Union(s2);
                    IntegerSet intersectionSet = s1.Intersection(s2);

                    // display results
                    UnionBox = unionSet.ToString();
                    IntersectionBox = intersectionSet.ToString();
                }
                else
                {
                    inputError("Please only enter integers separated by commas!");
                }
            }
        }

        public void inputError(string message)
        {
            StatusBox = message;
            UnionBox = "";
            IntersectionBox = "";
        }

        #region Input and Output Boxes
        private String _input1Box;
        public String Input1Box
        {
            get { return _input1Box; }
            set
            {
                _input1Box = value;

                OnPropertyChanged("Input1Box");
            }
        }

        private String _input2Box;
        public String Input2Box
        {
            get { return _input2Box; }
            set
            {
                _input2Box = value;

                OnPropertyChanged("Input2Box");
            }
        }

        private String _intersectionBox;
        public String IntersectionBox
        {
            get { return _intersectionBox; }
            set
            {
                _intersectionBox = value;

                OnPropertyChanged("IntersectionBox");
            }
        }

        private String _unionBox;
        public String UnionBox
        {
            get { return _unionBox; }
            set
            {
                _unionBox = value;

                OnPropertyChanged("UnionBox");
            }
        }

        private String _statusBox;
        public String StatusBox
        {
            get { return _statusBox; }
            set
            {
                _statusBox = value;

                OnPropertyChanged("StatusBox");
            }
        }
        #endregion

        #region Data Binding Stuff
        // define our property chage event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // implements method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion
    }
}
