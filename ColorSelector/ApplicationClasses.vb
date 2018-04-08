Imports System.Xml.Serialization
Imports ColorSelector
Imports Windows.Storage
Imports Windows.UI

Public Class ApplicationDataClass
    Implements INotifyPropertyChanged
#Region "Реализация интерфейса"
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub OnPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub
#End Region
    Private VisualSettingValue As New VisualSettingDataClass

    Public Property VisualSetting As VisualSettingDataClass
        Get
            Return VisualSettingValue
        End Get
        Set(value As VisualSettingDataClass)
            VisualSettingValue = value
            OnPropertyChanged("VisualSetting")
        End Set
    End Property
End Class


Public Class VisualSettingDataClass
    Implements INotifyPropertyChanged
#Region "Реализация интерфейса"
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub OnPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub
#End Region
    '!!!!!!!!! При добавлении новых сохроняемых свойств ОБЯЗАТЕЛЬНО прописать их в процедуре SetSavedValues
    Private LightColorValue As Color
    Private StandartColorValue As Color
    Private DarkColorValue As Color
    Private ForegroundColorValue As Color

    Private StylePresetsListValue As New ObservableCollection(Of StylePresetItemClass)
    Private StylePresetsSelectedIndexValue As Integer

    Public Sub New()
    End Sub
#Region "Свойства"
    Public Property StylePresetsSelectedIndex As Integer
        Get
            Return StylePresetsSelectedIndexValue
        End Get
        Set(value As Integer)
            StylePresetsSelectedIndexValue = value
            OnPropertyChanged("StylePresetsSelectedIndex")
            SetStyle(value)
        End Set
    End Property
    <Xml.Serialization.XmlIgnore>
    Public Property LightColor As Color
        Get
            Return LightColorValue
        End Get
        Set(value As Color)
            LightColorValue = value
            OnPropertyChanged("LightColor")
        End Set
    End Property
    <Xml.Serialization.XmlIgnore>
    Public Property StandartColor As Color
        Get
            Return StandartColorValue
        End Get
        Set(value As Color)
            StandartColorValue = value
            OnPropertyChanged("StandartColor")
        End Set
    End Property
    <Xml.Serialization.XmlIgnore>
    Public Property DarkColor As Color
        Get
            Return DarkColorValue
        End Get
        Set(value As Color)
            DarkColorValue = value
            OnPropertyChanged("DarkColor")
        End Set
    End Property
    <Xml.Serialization.XmlIgnore>
    Public Property ForegroundColor As Color
        Get
            Return ForegroundColorValue
        End Get
        Set(value As Color)
            ForegroundColorValue = value
            OnPropertyChanged("ForegroundColor")
        End Set
    End Property
    Public Property StylePresetsList As ObservableCollection(Of StylePresetItemClass)
        Get
            Return StylePresetsListValue
        End Get
        Set(value As ObservableCollection(Of StylePresetItemClass))
            StylePresetsListValue = value
            OnPropertyChanged("StylePresetsList")
        End Set
    End Property
#End Region
#Region "Процедуры и функции"
    ''' <summary>
    ''' Процедура наполнения списка цветовых стилей (запускается из SetOwner)
    ''' </summary>
    Private Sub AddStyle()
        StylePresetsList.Add(New StylePresetItemClass(ColorHelper.FromArgb(255, 117, 71, 25), ColorHelper.FromArgb(255, 73, 32, 12), ColorHelper.FromArgb(255, 51, 32, 17), Colors.Wheat))
        StylePresetsList.Add(New StylePresetItemClass(ColorHelper.FromArgb(255, 98, 142, 42), ColorHelper.FromArgb(255, 53, 85, 42), ColorHelper.FromArgb(255, 58, 63, 31), ColorHelper.FromArgb(255, 201, 221, 149)))
        StylePresetsList.Add(New StylePresetItemClass(ColorHelper.FromArgb(255, 147, 122, 148), ColorHelper.FromArgb(255, 114, 96, 114), ColorHelper.FromArgb(255, 86, 74, 82), ColorHelper.FromArgb(255, 241, 233, 238)))
        StylePresetsList.Add(New StylePresetItemClass(ColorHelper.FromArgb(255, 118, 144, 201), ColorHelper.FromArgb(255, 86, 105, 143), ColorHelper.FromArgb(255, 45, 37, 86), Colors.White))
        StylePresetsList.Add(New StylePresetItemClass(Colors.DarkGray, Colors.Gray, Colors.Black, Colors.White))
    End Sub
    Private Sub SetStyle(i As Integer)
        If StylePresetsList.Count = 0 Or i = -1 Then Exit Sub
        LightColor = StylePresetsList(i).LightColor
        StandartColor = StylePresetsList(i).StandartColor
        DarkColor = StylePresetsList(i).DarkColor
        ForegroundColor = StylePresetsList(i).ForegroundColor
    End Sub

    Public Sub SetValues(sd As VisualSettingDataClass)
        StylePresetsList.Clear()
        If sd.StylePresetsList.Count > 0 Then
            For Each l In sd.StylePresetsList
                StylePresetsList.Add(l)
            Next
            StylePresetsSelectedIndex = sd.StylePresetsSelectedIndex
        Else
            AddStyle()
            StylePresetsSelectedIndex = 0
        End If
    End Sub
#End Region
End Class
''' <summary>
''' Базовый класс для элемента цветового стиля
''' </summary>
Public Class StylePresetItemClass
    Public Sub New()

    End Sub
    Public Sub New(lc As Color, sc As Color, dc As Color, fc As Color)
        LightColor = lc
        StandartColor = sc
        DarkColor = dc
        ForegroundColor = fc
    End Sub
    Public Property LightColor As Color
    Public Property StandartColor As Color
    Public Property DarkColor As Color
    Public Property ForegroundColor As Color
End Class

''' <summary>
''' Сохранение и открытие настроек приложения
''' </summary>
Public Class SavedDataManager
    Public Shared Async Function SaveSettings(s As VisualSettingDataClass) As Task(Of Boolean)
        'Определяем путь к локальной папке
        Dim localFolder As StorageFolder = ApplicationData.Current.LocalFolder
        'Создаем сериализатор
        Dim Serializer As New XmlSerializer(GetType(VisualSettingDataClass))
        'Создаем врайтер для записи в файл
        Dim Part As New StringWriter
        'Создаем экземпляр класса для сохранения настроек
        Dim saveclass As New VisualSettingDataClass
        '\\\\Перенос настроек из данных приложения в класс сохранения
        saveclass.SetValues(s)
        '\\\\
        'Сериализуем клас с настроками во врайтер
        Serializer.Serialize(Part, saveclass)
        'Создаем файл, куда будут записаны настройки
        Dim DataFile As StorageFile = Await localFolder.CreateFileAsync("SettingsFile.dat", CreationCollisionOption.ReplaceExisting)
        'Пишем настройки
        Await FileIO.WriteTextAsync(DataFile, Part.ToString)
        Return True
    End Function
    ''' <summary>
    ''' Загрузка настроек, актуальных только на этом устройстве
    ''' </summary>
    ''' <returns></returns>
    Public Shared Async Function LoadSettings() As Task(Of VisualSettingDataClass)
        'Определяем путь к локальной папке
        Dim localFolder As StorageFolder = ApplicationData.Current.LocalFolder
        'Удаляем файл, если нужно очистить настройеки
        'Dim fil As Windows.Storage.StorageFile = Await localFolder.GetFileAsync("SettingsFile.dat")
        'Await fil.DeleteAsync()
        'Загружаем фал из локального хранилища.
        Dim DataFile As StorageFile = Await localFolder.TryGetItemAsync("SettingsFile.dat")
        If DataFile Is Nothing Then Return New VisualSettingDataClass
        'Захружаем XML в строку
        Dim SerializedString As String = Await FileIO.ReadTextAsync(DataFile)
        'Создаем сериализатор
        Dim reader = New XmlSerializer(GetType(VisualSettingDataClass))
        'Создаем ридер
        Dim sr As New StringReader(SerializedString)
        'Десериализуем из ридера в класс
        Dim saveclass As VisualSettingDataClass = reader.Deserialize(sr)
        Return saveclass
    End Function
End Class