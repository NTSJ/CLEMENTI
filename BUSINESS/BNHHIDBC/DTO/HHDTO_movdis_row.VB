Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class HHDTO_movdis_row
    Inherits DTO__BASE

    Public Property UID_Padre As String
    Public Property UID_Figlio As String

    ''' <summary>
    ''' anagrafica articolo padre
    ''' </summary>
    ''' <returns></returns>
    Public Property oCodicePadre As New HHDTO_articolo


    ''' <summary>
    ''' anagrafica articol figlio
    ''' </summary>
    ''' <returns></returns>
    Public Property oCodiceFiglio As New HHDTO_articolo

    ''' <summary>
    ''' BOM_QTY
    ''' </summary>
    ''' <returns></returns>
    Public Property Quantita As Decimal
    ''' <summary>
    ''' BOM_POS
    ''' </summary>
    ''' <returns></returns>
    Public Property Posizione As String


    ''' <summary>
    ''' Indica se il componente è la "materia prima" in DBCent, ossia l'ultimo ramo gestito
    ''' </summary>
    ''' <returns></returns>
    Public Property UltimoLivello As Boolean


End Class
