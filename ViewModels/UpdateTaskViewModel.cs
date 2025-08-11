using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfScheduledApp20250729;
using WpfScheduledApp20250729.Models;
using WpfScheduledApp20250729.Models.Entities;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Validation;

namespace WpfScheduledApp20250729.ViewModels
{
    internal class UpdateTaskViewModel : NotificationObject
    {
        private readonly object? _originalEntity;
        private HighTaskService? _highTaskService;
        private MiddleTaskService? _middleTaskService;
        private LowTaskService? _lowTaskService;
        private string _windowTitle = "Edit Task";

        public ObservableCollection<FormFieldModel> FormFields { get; } = new();

        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public UpdateTaskViewModel(int taskId)
        {
            // Legacy constructor for backwards compatibility
            _originalEntity = null;
        }

        public UpdateTaskViewModel(object taskEntity)
        {
            _originalEntity = taskEntity;
            InitializeServices();
            CreateFormFields();
            PopulateFormFields();
        }

        private void InitializeServices()
        {
            // In a real application, these would be injected via DI
            // For now, we'll create them when needed
            var context = new Models.Context.DevelopmentContext();
            _highTaskService = new HighTaskService(context);
            _middleTaskService = new MiddleTaskService(context);
            _lowTaskService = new LowTaskService(context);
        }

        private void CreateFormFields()
        {
            FormFields.Clear();

            if (_originalEntity == null)
            {
                WindowTitle = "Edit Task";
                return;
            }

            switch (_originalEntity)
            {
                case HighTask highTask:
                    WindowTitle = "Edit High Task";
                    CreateHighTaskFields();
                    break;
                case MiddleTask middleTask:
                    WindowTitle = "Edit Middle Task";
                    CreateMiddleTaskFields();
                    break;
                case LowTask lowTask:
                    WindowTitle = "Edit Low Task";
                    CreateLowTaskFields();
                    break;
                default:
                    WindowTitle = "Edit Task";
                    break;
            }
        }

        private void CreateHighTaskFields()
        {
            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.TaskName),
                DisplayName = "Task Name",
                FieldType = FormFieldType.Text,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MaxLengthValidationRule(200)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.Description),
                DisplayName = "Description",
                FieldType = FormFieldType.TextArea,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MaxLengthValidationRule(1000)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.ArchitectureId),
                DisplayName = "Architecture ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.ProjectId),
                DisplayName = "Project ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.ClearTimesInTime),
                DisplayName = "Clear Times In Time",
                FieldType = FormFieldType.Number,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MinValueValidationRule(0)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(HighTask.ClearTimesOutofTime),
                DisplayName = "Clear Times Out of Time",
                FieldType = FormFieldType.Number,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MinValueValidationRule(0)
                }
            });
        }

        private void CreateMiddleTaskFields()
        {
            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.HighTaskId),
                DisplayName = "High Task ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.HtlId),
                DisplayName = "HTL ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.ProjectId),
                DisplayName = "Project ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.Description),
                DisplayName = "Description",
                FieldType = FormFieldType.TextArea,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MaxLengthValidationRule(1000)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.CanAutoReschedule),
                DisplayName = "Can Auto Reschedule",
                FieldType = FormFieldType.Boolean,
                IsRequired = false
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.EstimatedTime),
                DisplayName = "Estimated Time (minutes)",
                FieldType = FormFieldType.Number,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MinValueValidationRule(0)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.Url),
                DisplayName = "URL",
                FieldType = FormFieldType.Text,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MaxLengthValidationRule(500)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(MiddleTask.FileName),
                DisplayName = "File Name",
                FieldType = FormFieldType.Text,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MaxLengthValidationRule(255)
                }
            });
        }

        private void CreateLowTaskFields()
        {
            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.MiddleTaskId),
                DisplayName = "Middle Task ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.ProjectId),
                DisplayName = "Project ID",
                FieldType = FormFieldType.Number,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule(),
                    new MinValueValidationRule(1)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.EstimatedTime),
                DisplayName = "Estimated Time (minutes)",
                FieldType = FormFieldType.Number,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MinValueValidationRule(0)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.Description),
                DisplayName = "Description",
                FieldType = FormFieldType.TextArea,
                IsRequired = false,
                ValidationRules = new List<ValidationRule>
                {
                    new MaxLengthValidationRule(1000)
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.ExecutionDate),
                DisplayName = "Execution Date",
                FieldType = FormFieldType.Date,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule()
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.ExecutionTime),
                DisplayName = "Execution Time",
                FieldType = FormFieldType.Time,
                IsRequired = true,
                ValidationRules = new List<ValidationRule>
                {
                    new RequiredValidationRule()
                }
            });

            FormFields.Add(new FormFieldModel
            {
                PropertyName = nameof(LowTask.CanAutoReschedule),
                DisplayName = "Can Auto Reschedule",
                FieldType = FormFieldType.Boolean,
                IsRequired = false
            });
        }

        private void PopulateFormFields()
        {
            if (_originalEntity == null) return;

            var entityType = _originalEntity.GetType();

            foreach (var field in FormFields)
            {
                var property = entityType.GetProperty(field.PropertyName);
                if (property != null)
                {
                    field.Value = property.GetValue(_originalEntity);
                }
            }
        }

        private DelegateCommand? _sendCommand;
        public DelegateCommand SendCommand
        {
            get
            {
                return _sendCommand ??= new DelegateCommand(
                    async _ => await SaveChangesAsync(),
                    _ => true);
            }
        }

        private DelegateCommand? _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(
                    _ => CloseWindow(),
                    _ => true);
            }
        }

        private async Task SaveChangesAsync()
        {
            // Validate all fields
            var hasErrors = false;
            foreach (var field in FormFields)
            {
                var result = field.Validate();
                if (!result.IsValid)
                {
                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                MessageBox.Show("Please fix all validation errors before saving.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Update the original entity with form values
                UpdateEntityFromForm();

                // Save to database based on entity type
                var success = await SaveEntityAsync();
                
                if (success)
                {
                    MessageBox.Show("Task updated successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                }
                else
                {
                    MessageBox.Show("Failed to update task. Please try again.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateEntityFromForm()
        {
            if (_originalEntity == null) return;

            var entityType = _originalEntity.GetType();

            foreach (var field in FormFields)
            {
                var property = entityType.GetProperty(field.PropertyName);
                if (property != null && property.CanWrite)
                {
                    var value = field.Value;
                    
                    // Type conversion if needed
                    if (value != null && property.PropertyType != value.GetType())
                    {
                        if (property.PropertyType == typeof(int) && int.TryParse(value.ToString(), out var intValue))
                            value = intValue;
                        else if (property.PropertyType == typeof(bool) && bool.TryParse(value.ToString(), out var boolValue))
                            value = boolValue;
                        else if (property.PropertyType == typeof(DateOnly) && DateOnly.TryParse(value.ToString(), out var dateValue))
                            value = dateValue;
                        else if (property.PropertyType == typeof(TimeOnly) && TimeOnly.TryParse(value.ToString(), out var timeValue))
                            value = timeValue;
                    }

                    property.SetValue(_originalEntity, value);
                }
            }

            // Update common BaseEntity fields
            if (_originalEntity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;
                baseEntity.TouchedAt = DateTime.UtcNow;
                baseEntity.LastUpdMethodName = "UpdateTaskWindow";
            }
        }

        private async Task<bool> SaveEntityAsync()
        {
            if (_originalEntity == null) return false;

            return _originalEntity switch
            {
                HighTask highTask when _highTaskService != null => await _highTaskService.UpdateAsync(highTask.Id, highTask) != null,
                MiddleTask middleTask when _middleTaskService != null => await _middleTaskService.UpdateAsync(middleTask.Id, middleTask) != null,
                LowTask lowTask when _lowTaskService != null => await _lowTaskService.UpdateAsync(lowTask.Id, lowTask) != null,
                _ => false
            };
        }

        private void CloseWindow()
        {
            // This would need to be implemented based on how window closing is handled
            // For now, we'll use the Application's main window approach
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event EventHandler? RequestClose;
        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}