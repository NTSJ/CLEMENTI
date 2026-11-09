Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*__CLIE
Public Class FRO__CLIE
  Inherits FRM__CLIE

  Public _oClfClie As CLF__CLIE
    Public Property oClfClie() As CLF__CLIE
        Get
            If _oClfClie Is Nothing Then _oClfClie = CType(ocleClie, CLF__CLIE)
            Return _oClfClie
        End Get
        Set(ByVal value As CLF__CLIE)
            _oClfClie = value
        End Set
    End Property

    Public WithEvents edAn_hhdatainizioassicur_form As NTSTextBoxData
    Public WithEvents edXx_hhdatafineassicur_form As NTSTextBoxData

    Public Overrides Sub InitControls()
        MyBase.InitControls()

        Try
            HHInitControlsPers()
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub HHInitControlsPers()
        Try
            edAn_hhdatainizioassicur_form = CType(NTSFindControlByName(Me, "edAn_hhdatainizioassicur"), NTSTextBoxData)
            edXx_hhdatafineassicur_form = CType(NTSFindControlByName(Me, "edXx_hhdatafineassicur"), NTSTextBoxData)
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overrides Sub GestisciEventiEntity(sender As Object, ByRef e As NTSEventArgs)

        Try
            If e.TipoEvento.ToUpper = "HHASSICURAZIONE_CAMPI_DATA_ENABLE" Then
                Dim bEnable As Boolean = CType(e.Message, Boolean)
                edAn_hhdatainizioassicur_form.Enabled = bEnable
                edXx_hhdatafineassicur_form.Enabled = bEnable

                If bEnable Then
                    edAn_hhdatainizioassicur_form.BackColor = Color.Empty
                    edXx_hhdatafineassicur_form.BackColor = Color.Empty
                Else
                    edAn_hhdatainizioassicur_form.BackColor = Color.LightGray
                    edXx_hhdatafineassicur_form.BackColor = Color.LightGray
                End If

                Return
            End If
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
        MyBase.GestisciEventiEntity(sender, e)

    End Sub

End Class

