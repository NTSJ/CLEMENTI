Imports System.Data
Imports NTSInformatica.CLN__STD

Imports System
'Ricordarsi di inserire in DLLMAP.INI la direttiva +B*__STFI
Public Class FRO__STFI
  Inherits FRM__STFI

  Public _oClfStfi As CLF__STFI
  Public Property oClfStfi() As CLF__STFI
    Get
      If _oClfStfi Is Nothing Then _oClfStfi = CType(ocleStfi, CLF__STFI)
      Return _oClfStfi
    End Get
    Set(ByVal value As CLF__STFI)
      _oClfStfi = value
    End Set
  End Property

End Class

