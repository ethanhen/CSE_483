using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApp2
{
    class Model : INotifyPropertyChanged
    {

        public void Update(string op)
        {
            float inputLeft, inputRight, result;

            float.TryParse(InputBox_Left, out inputLeft);
            float.TryParse(InputBox_Right, out inputRight);

            switch (op)
            {
                case "addButtn":
                    result = inputLeft + inputRight;
                    break;
                case "subButtn":
                    result = inputLeft - inputRight;
                    break;
                case "mulButtn":
                    result = inputLeft * inputRight;
                    break;
                case "divButtn":
                    result = inputLeft / inputRight;
                    break;
                default:
                    result = 0;
                    break;

            } 

            OutputBox = result.ToString();
        }

        private String _inputBox_Left;
        public String InputBox_Left
        {
            get { return _inputBox_Left; }
            set
            {
                _inputBox_Left = value;

                OnPropertyChanged("InputBox_Left");
            }
        }

        private String _inputBox_Right;
        public String InputBox_Right
        {
            get { return _inputBox_Right; }
            set
            {
                _inputBox_Right = value;

                OnPropertyChanged("InputBox_Right");
            }
        }

        private String _outputBox;
        public String OutputBox
        {
            get { return _outputBox; }
            set
            {
                _outputBox = value;

                OnPropertyChanged("OutputBox");
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
