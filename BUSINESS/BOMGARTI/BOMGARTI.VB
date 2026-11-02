Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*MGARTI
Public Class FROMGARTI
  Inherits FRMMGARTI

  Public _oClfArti As CLFMGARTI
    Public Property oClfArti() As CLFMGARTI
        Get
            If _oClfArti Is Nothing Then _oClfArti = CType(ocleArti, CLFMGARTI)
            Return _oClfArti
        End Get
        Set(ByVal value As CLFMGARTI)
            _oClfArti = value
        End Set
    End Property


    Public WithEvents ckAr_hhflgestraee_form As NTSCheckBox
    Public WithEvents pnHHGestRaee_form As NTSPanel

    Public Overrides Sub InitControls()
        MyBase.InitControls()

        Try
            HHInitControlsPers()
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try

    End Sub



    Private Sub HHInitControlsPers()
        Try
            ckAr_hhflgestraee_form = CType(NTSFindControlByName(Me, ("ckAr_hhflgestraee")), NTSCheckBox)
            pnHHGestRaee_form = CType(NTSFindControlByName(Me, ("pnHHGestRaee")), NTSPanel)
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub

    Public Overrides Sub GestisciEventiEntity(sender As Object, ByRef e As NTSEventArgs)

        Try

            Select Case e.TipoEvento.ToUpper
                Case "HHGEST_RAEE_ABILITATA"
                    ' gestione raee attivata
                    pnHHGestRaee_form.Enabled = True
                Case "HHGEST_RAEE_DISABILITATA"
                    ' gestione raee disattivata
                    pnHHGestRaee_form.Enabled = False
                Case Else
                    ' standard
                    MyBase.GestisciEventiEntity(sender, e)
            End Select
        Catch ex As Exception
            CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try

    End Sub


End Class

