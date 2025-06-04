public class Passenger
{
    public (int x, int y) Pickup;
    public (int x, int y) Dropoff;

    public Passenger(int px, int py, int dx, int dy)
    {
        Pickup = (px, py);
        Dropoff = (dx, dy);
    }
}