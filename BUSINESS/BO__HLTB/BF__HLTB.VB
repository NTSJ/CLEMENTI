Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Globalization


Imports System
Public Class CLF__HLTB
  Inherits CLE__HLTB

  Public _oClhHltb As CLH__HLTB
  Public Property oClhHltb() As CLH__HLTB
    Get
      If _oClhHltb Is Nothing Then _oClhHltb = CType(ocldHltb, CLH__HLTB)
      Return _oClhHltb
    End Get
    Set(ByVal value As CLH__HLTB)
      _oClhHltb = value
    End Set
  End Property

End Class