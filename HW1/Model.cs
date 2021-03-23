﻿using System;
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
        public void Update()
        {
            StatusBox = "";

            string[] set1String = null;
            string[] set2String = null;

            List<int> set1 = new List<int>();
            List<int> set2 = new List<int>();

            int temp;

            IntegerSet s1 = new IntegerSet();
            IntegerSet s2 = new IntegerSet();


            if (Input1Box == null || Input2Box == null || Input1Box == "" || Input2Box == "")
            {
                StatusBox = "You cannot enter empty sets!";
                UnionBox = "";
                IntersectionBox = "";
            }
            else
            {
                set1String = Input1Box.Split(',');
                set2String = Input2Box.Split(',');

                foreach(string x in set1String)
                {
                    if (!int.TryParse(x, out temp))
                    {
                        StatusBox = "Please only enter integers separated by commas!";
                        UnionBox = "";
                        IntersectionBox = "";
                        break;
                    }
                    else
                    {
                        set1.Add(temp);
                    }
                }

                foreach (string x in set2String)
                {
                    if (!int.TryParse(x, out temp))
                    {
                        StatusBox = "Please only enter integers separated by commas!";
                        UnionBox = "";
                        IntersectionBox = "";
                        break;
                    }
                    else
                    {
                        set2.Add(temp);
                    }
                }

                foreach(int x in set1)
                {
                    s1.InsertElement(x);
                }

                foreach (int x in set2)
                {
                    s2.InsertElement(x);
                }

                IntegerSet unionSet = s1.Union(s2);
                IntegerSet intersectionSet = s1.Intersection(s2);

                UnionBox = unionSet.ToString();
                IntersectionBox = intersectionSet.ToString();

            }
        }

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
