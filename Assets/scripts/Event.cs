class Event : MonoBehaviour {
    bool active;
    
    void Setup() {
    }
    
    void Update(ref Body body) {
        body.bloodAmount -= 1.0;
    }
}

//Each separate event subclass should be placed in it's own script,
//    on a certain part of the body
class CutOpenEvent : Event {
}
