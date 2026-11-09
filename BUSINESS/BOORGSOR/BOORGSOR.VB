Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
Imports DevExpress.XtraGrid.Views.Base
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*ORGSOR
Public Class FROORGSOR
    Inherits FRMORGSOR

    Public _oClfGsor As CLFORGSOR
    Public Property oClfGsor() As CLFORGSOR
        Get
            If _oClfGsor Is Nothing Then _oClfGsor = CType(oCleGsor, CLFORGSOR)
            Return _oClfGsor
        End Get
        Set(ByVal value As CLFORGSOR)
            _oClfGsor = value
        End Set
    End Property

End Class

