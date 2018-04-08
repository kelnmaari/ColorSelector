''' <summary>
''' Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
''' </summary>
NotInheritable Class App
    Inherits Application

    ''' <summary>
    ''' Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
    ''' если приложение запускается для открытия конкретного файла, отображения
    ''' результатов поиска и т. д.
    ''' </summary>
    ''' <param name="e">Сведения о запросе и обработке запуска.</param>
    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
        ' только обеспечьте активность окна

        If rootFrame Is Nothing Then
            ' Создание фрейма, который станет контекстом навигации, и переход к первой странице
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Загрузить состояние из ранее приостановленного приложения
            End If
            ' Размещение фрейма в текущем окне
            Window.Current.Content = rootFrame
        End If

        If e.PrelaunchActivated = False Then
            If rootFrame.Content Is Nothing Then
                ' Если стек навигации не восстанавливается для перехода к первой странице,
                ' настройка новой страницы путем передачи необходимой информации в качестве параметра
                ' параметр
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            ' Обеспечение активности текущего окна
            Window.Current.Activate()
        End If
    End Sub

    ''' <summary>
    ''' Вызывается в случае сбоя навигации на определенную страницу
    ''' </summary>
    ''' <param name="sender">Фрейм, для которого произошел сбой навигации</param>
    ''' <param name="e">Сведения о сбое навигации</param>
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ''' <summary>
    ''' Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
    ''' без учета информации о том, будет ли оно завершено или возобновлено с неизменным
    ''' содержимым памяти.
    ''' </summary>
    ''' <param name="sender">Источник запроса приостановки.</param>
    ''' <param name="e">Сведения о запросе приостановки.</param>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' TODO: Сохранить состояние приложения и остановить все фоновые операции
        deferral.Complete()
    End Sub

    Public Sub SetTitleBar(a As VisualSettingDataClass)
        Dim tb As ApplicationViewTitleBar = ApplicationView.GetForCurrentView.TitleBar
        'Устанавливает ФОН заголовка, но не затрагивает часть с кнопками управления окном.
        tb.BackgroundColor = a.StandartColor
        'Устанавливает ФОН под кнопками управления окном
        tb.ButtonBackgroundColor = a.StandartColor
        'Устанавливает цвет СИМВОЛОВ внутри кнопок управления окном (чертачка, крестик, квадратик).
        tb.ButtonForegroundColor = a.ForegroundColor
        'Устанавливает ФОН кнопок управления окном в момент наведния на них мыши. ВНИМАНИЕ! Не влияет на кнопку закрытия окна.
        tb.ButtonHoverBackgroundColor = a.LightColor
        'Устанавливает цвет СИМВОЛОВ внутри кнопок управления окном в момент наведния на них мыши. ВНИМАНИЕ! Не влияет на кнопку закрытия окна.
        tb.ButtonHoverForegroundColor = a.ForegroundColor
        'Устанавливает ФОН кнопок управления окном в момент когда приложение не активно
        tb.ButtonInactiveBackgroundColor = a.LightColor
        'Устанавливает цвет СИМВОЛОВ внутри кнопок управления окном в момент когда приложение не активно
        tb.ButtonInactiveForegroundColor = a.StandartColor
        'Устанавливает ФОН кнопок управления окном в момент нажатия на них. ВНИМАНИЕ! Не влияет на кнопку закрытия окна.
        tb.ButtonPressedBackgroundColor = a.DarkColor
        'Устанавливает цвет СИМВОЛОВ внутри кнопок управления окном в момент нажатия на них. ВНИМАНИЕ! Не влияет на кнопку закрытия окна.
        tb.ButtonPressedForegroundColor = a.ForegroundColor
        'Устанавливает цвет СИМВОЛОВ заголовка окна
        tb.ForegroundColor = a.ForegroundColor
        'Устанавливает ФОН заголовка в момент когда приложение не активно
        tb.InactiveBackgroundColor = a.LightColor
        'Устанавливает цвет СИМВОЛОВ заголовка окна в момент когда приложение не активно
        tb.InactiveForegroundColor = a.StandartColor
    End Sub
End Class