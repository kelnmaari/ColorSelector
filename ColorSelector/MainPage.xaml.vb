Public NotInheritable Class MainPage
    Inherits Page
    Dim MyApp As App = Application.Current
    Private MyApplicationData As ApplicationDataClass = DirectCast(Application.Current.Resources("ApplicationData"), ApplicationDataClass)
    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        MyApplicationData.VisualSetting.SetValues(Await SavedDataManager.LoadSettings())
        MyApp.SetTitleBar(MyApplicationData.VisualSetting)
        AddHandler Application.Current.Suspending, AddressOf SuspendingApp
        SelectColorGridView_SelectionChanged()
    End Sub

    Private Async Sub SuspendingApp(sender As Object, e As SuspendingEventArgs)
        Await SavedDataManager.SaveSettings(MyApplicationData.VisualSetting)
    End Sub
    Private Sub StyleGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MyApp.SetTitleBar(MyApplicationData.VisualSetting)
        SelectColorGridView_SelectionChanged()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs)
        MyApplicationData.VisualSetting.StylePresetsList.Add(New StylePresetItemClass(CType(CType(SelectColorGridView.Items(0), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(1), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(2), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(3), Border).Background, SolidColorBrush).Color))
    End Sub

    Private Sub ChengeButton_Click(sender As Object, e As RoutedEventArgs)
        Dim i As Integer = StyleGridView.SelectedIndex
        MyApplicationData.VisualSetting.StylePresetsList.RemoveAt(i)
        MyApplicationData.VisualSetting.StylePresetsList.Insert(i, New StylePresetItemClass(CType(CType(SelectColorGridView.Items(0), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(1), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(2), Border).Background, SolidColorBrush).Color, CType(CType(SelectColorGridView.Items(3), Border).Background, SolidColorBrush).Color))
        StyleGridView.SelectedIndex = i
    End Sub

    Private Sub DeliteButton_Click(sender As Object, e As RoutedEventArgs)
        MyApplicationData.VisualSetting.StylePresetsList.RemoveAt(StyleGridView.SelectedIndex)
        If StyleGridView.Items.Count = 0 Then Exit Sub
        StyleGridView.SelectedIndex = 0
    End Sub

    Private Sub ColorPicker_ColorChanged(sender As ColorPicker, args As ColorChangedEventArgs)
        Select Case SelectColorGridView.SelectedIndex
            Case 0
                MyApplicationData.VisualSetting.LightColor = args.NewColor
            Case 1
                MyApplicationData.VisualSetting.StandartColor = args.NewColor
            Case 2
                MyApplicationData.VisualSetting.DarkColor = args.NewColor
            Case 3
                MyApplicationData.VisualSetting.ForegroundColor = args.NewColor
        End Select
    End Sub

    Private Sub SelectColorGridView_SelectionChanged()
        If SelectColorGridView.SelectedIndex > -1 And CType(SelectColorGridView.SelectedItem, Border).Background IsNot Nothing Then
            SelectColorPicker.Color = CType(CType(SelectColorGridView.SelectedItem, Border).Background, SolidColorBrush).Color
        End If
    End Sub
End Class