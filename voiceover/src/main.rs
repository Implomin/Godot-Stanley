mod action;
use action::Action;

fn main() {


    let action = Action::TURN("left".to_string());
    print!("{}", action.value("Make a short funny, a bit mean, narration about a player who: ".to_string()))
    
}
