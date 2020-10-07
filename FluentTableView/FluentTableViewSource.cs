using System;
using System.Windows.Input;
using Foundation;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace FluentTableView
{
    public class FluentTableViewSource : MvxTableViewSource
    {
        public ICommand? DeleteRowCommand { get; set; }
        
        private readonly MvxIosMajorVersionChecker _iosVersion6Checker = new MvxIosMajorVersionChecker(6);

        private NSString CellIdentifier { get; }
        
        public FluentTableViewSource(UITableView tableView, Type cellType, string? cellIdentifier = null,  bool registerForCellReuse = true) : base(tableView)
        {
            // if no cellIdentifier supplied, then use the class name as cellId
            cellIdentifier ??= nameof(cellType);
            CellIdentifier = new NSString(cellIdentifier);

            if (registerForCellReuse)
            {
                tableView.RegisterClassForCellReuse(cellType, CellIdentifier);
            }
        }
        
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (_iosVersion6Checker.IsVersionOrHigher)
                return tableView.DequeueReusableCell(CellIdentifier, indexPath);

            return tableView.DequeueReusableCell(CellIdentifier);
        }
        
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }
 
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (DeleteRowCommand != null && DeleteRowCommand.CanExecute(GetItemAt(indexPath)))
            {
                return UITableViewCellEditingStyle.Delete;
            }
     
            return UITableViewCellEditingStyle.None;
        }
        
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        { 
            switch (editingStyle) 
            { 
                case UITableViewCellEditingStyle.Delete:
                    DeleteRowCommand?.Execute(GetItemAt(indexPath));
                    break; 
                case UITableViewCellEditingStyle.None: 
                    break; 
            }
        }
    }
}