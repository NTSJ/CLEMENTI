Imports System.Data
Imports NTSInformatica.CLN__STD

''' <summary>
''' Dati assicurazione collegata al cliente
''' </summary>
<Serializable()>
Public Class HHDTO_Assicurazione
    Inherits DTO__BASE

    Private _nConto As Integer
    ''' <summary>
    ''' Codice cliente
    ''' </summary>
    ''' <returns></returns>
    Public Property nConto() As Integer
        Get
            Return _nConto
        End Get
        Set(ByVal value As Integer)
            _nConto = value
        End Set
    End Property

    Private _nCodice As Integer
    ''' <summary>
    ''' codice assicurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property nCodice() As Integer
        Get
            Return _nCodice
        End Get
        Set(ByVal value As Integer)
            _nCodice = value
        End Set
    End Property

    Private _strDescrizione As String
    ''' <summary>
    ''' Descrizione
    ''' </summary>
    ''' <returns></returns>
    Public Property strDescrizione() As String
        Get
            Return _strDescrizione
        End Get
        Set(ByVal value As String)
            _strDescrizione = value
        End Set
    End Property

    Private _nTipo As Integer
    ''' <summary>
    ''' Tipo assicurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property nTipo() As Integer
        Get
            Return _nTipo
        End Get
        Set(ByVal value As Integer)
            _nTipo = value
        End Set
    End Property

    Private _bGestita_a_scadenza As Boolean
    ''' <summary>
    ''' Soggetta a scadenza
    ''' </summary>
    ''' <returns></returns>
    Public Property bGestita_a_scadenza() As Boolean
        Get
            Return _bGestita_a_scadenza
        End Get
        Set(ByVal value As Boolean)
            _bGestita_a_scadenza = value
        End Set
    End Property

    Private _nMesiValidita As Integer
    ''' <summary>
    ''' Numero mesi valdità dell'assicurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property nMesiValidita() As Integer
        Get
            Return _nMesiValidita
        End Get
        Set(ByVal value As Integer)
            _nMesiValidita = value
        End Set
    End Property

    Private _dDataInizioVal As Date
    ''' <summary>
    ''' Data inizio validità 
    ''' </summary>
    ''' <returns></returns>
    Public Property dDataInizioVal() As Date
        Get
            Return _dDataInizioVal
        End Get
        Set(ByVal value As Date)
            _dDataInizioVal = value
        End Set
    End Property

    Private _dDataFineVal As Date
    ''' <summary>
    ''' Solo se l'assicurazione è gestita a scadenza, la data di fine validità viene calcolata a partire dalla (dDataInizioVal + nMesiValdita -1 gg)
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property dDataFineVal() As Date
        Get
            ' calcolo la data di fine validità
            If bGestita_a_scadenza Then
                _dDataFineVal = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, nMesiValidita, dDataInizioVal))
            Else
                _dDataFineVal = NTSCDate("31/12/2099")
            End If
            Return _dDataFineVal
        End Get
    End Property

End Class
