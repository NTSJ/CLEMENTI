Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Globalization


Imports System
Public Class CLF__STFI
  Inherits CLE__STFI

#Region "proprietà e variabili"


    Public _oClhStfi As CLH__STFI
    Public Property oClhStfi() As CLH__STFI
        Get
            If _oClhStfi Is Nothing Then _oClhStfi = CType(oCldStfi, CLH__STFI)
            Return _oClhStfi
        End Get
        Set(ByVal value As CLH__STFI)
            _oClhStfi = value
        End Set
    End Property
#End Region

#Region "routine standard"


    Public Overrides Function ApriStoricoFido(lConto As Integer, ByRef ds As DataSet) As Boolean
        Dim bResult As Boolean = MyBase.ApriStoricoFido(lConto, ds)

        Try
            If Not bResult Then Return False

            HHAddColumnsPers(ds)
            HHDecodColumnsPers(ds)


        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try


        Return bResult
    End Function
#End Region

#Region "routine personalizzate"


    ''' <summary>
    ''' Aggiunge colonne personalizzata al datatable dsshared.table("TABSTFI")
    ''' </summary>
    ''' <param name="ds"></param>
    Public Overridable Sub HHAddColumnsPers(ByRef ds As DataSet)
        Try
            If ds Is Nothing Then Return
            If Not ds.Tables.Contains("TABSTFI") Then Return

            If Not ds.Tables("TABSTFI").Columns.Contains("xx_hhdestipoassicur") Then
                ds.Tables("TABSTFI").Columns.Add(New DataColumn("xx_hhdestipoassicur", GetType(String)))
            End If

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        End Try
    End Sub

    ''' <summary>
    ''' Decodifica la descrizione dei campi personalizzati
    ''' </summary>
    ''' <param name="ds"></param>
    Public Overridable Sub HHDecodColumnsPers(ByRef ds As DataSet)

        Try

            For Each dtr As DataRow In dsShared.Tables("TABSTFI").Select($"tb_hhcodassicurazione <> 0")
                Dim dtrDesc() As DataRow = dsShared.Tables("HH_TIPIASSICURAZIONE").Select($"tb_codtipoassicur={CDblSQL(dtr!tb_hhcodassicurazione)}")
                If dtrDesc.Length > 0 Then dtr!xx_hhdestipoassicur = dtrDesc(0)!tb_destipoassicur.ToString
            Next

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, oApp.InfoError)
        Finally

        End Try
    End Sub

#End Region

End Class