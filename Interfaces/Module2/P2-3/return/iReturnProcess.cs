namespace ProRental.Interfaces.Domain;

public interface iReturnProcess
{
    bool TriggerReturnProcess(int orderId);
}