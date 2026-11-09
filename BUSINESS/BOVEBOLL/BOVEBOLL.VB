Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*VEBOLL
Public Class FROVEBOLL
  Inherits FRMVEBOLL

  Public _oClfBoll As CLFVEBOLL
    Public Property oClfBoll() As CLFVEBOLL
        Get
            If _oClfBoll Is Nothing Then _oClfBoll = CType(ocleBoll, CLFVEBOLL)
            Return _oClfBoll
        End Get
        Set(ByVal value As CLFVEBOLL)
            _oClfBoll = value
        End Set
    End Property

    Public Overrides Sub GestisciEventiEntity(sender As Object, ByRef e As NTSEventArgs)

        If e.Message.ToLower.Contains("Il documento è collegato alla fattura elettronica".ToLower) Then
            ' se la fattura è presente nelle fatture elettroniche non faccio partire la gestione del RAEE
            oClfBoll.bHHXmlEstratto = True
        End If

        MyBase.GestisciEventiEntity(sender, e)
    End Sub

End Class

