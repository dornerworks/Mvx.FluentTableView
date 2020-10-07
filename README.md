# FluentTableView
Use [Cirrious.FluentLayout](https://github.com/FluentLayout/Cirrious.FluentLayout) with MvvmCross TableViewSource and UITableViewCells.

```C#
public class ExampleCell : FluentTableViewCell
{
    private readonly UILabel _label = new UILabel();
    
    public LabelCell(IntPtr handle) : base(handle)
    {
    }

    protected override void SetupConstraints()
    {
        ContentView.AddSubviews(_label);
        ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        ContentView.AddConstraints(
            _label.AtLeftOf(ContentView),
            _label.AtTopOf(ContentView),
            _label.AtRightOf(ContentView),
            _label.AtBottomOf(ContentView));
    }

    protected override void Bind()
    {
        var set = this.CreateBindingSet<ExampleCell, ExampleViewModel>();
        set.Bind(_label)
            .To(vm => vm.Label);
        set.Apply();
    }
}
```

```C#
var tableView = new UITableView();
var source = new FluentTableViewSource(tableView, typeof(ExampleCell));
tableView.Source = source;

var set = CreateBindingSet();
set.Bind(source).To(vm => vm.Items);
set.Apply();
```
