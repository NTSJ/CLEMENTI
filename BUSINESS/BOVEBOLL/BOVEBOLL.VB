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


End Class

