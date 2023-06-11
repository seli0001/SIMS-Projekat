using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIMS.Core
{
    public abstract class ValidationBase: BindableBase
    {
        private ValidationErrors _validationErrors;
        public ValidationErrors ValidationErrors
        {
            get { return _validationErrors; }
            set
            {
                _validationErrors = value;
                OnPropertyChanged(nameof(ValidationErrors));
            }
        }
        public bool IsValid { get; private set; }

        protected ValidationBase()
        {
            this.ValidationErrors = new ValidationErrors();
        }

        protected abstract void ValidateSelf();

        public void Validate()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf();
            this.IsValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }
    }
}
