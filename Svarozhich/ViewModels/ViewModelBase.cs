using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;

namespace Svarozhich.ViewModels;

[DataContract(IsReference = true)]
public class ViewModelBase : ReactiveValidationObject
{
}