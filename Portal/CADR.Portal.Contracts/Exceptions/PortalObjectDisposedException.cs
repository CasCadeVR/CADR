namespace CADR.Portal.Contracts.Exceptions;

/// <summary>
/// Объект использовали после вызова Dispose
/// </summary>
public class PortalObjectDisposedException(object disposedObject) : Exception($"{disposedObject.GetType().Name} был использован после вызова Dispose")
{

}
