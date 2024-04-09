use serde::de::{Deserializer, IntoDeserializer};
use serde::Deserialize;


// #[derive(Deserialize)]
pub(crate) enum Action {
    TURN,
    SEE,
    WALK,
    PICK,
    STAND,
    OTHER
}


impl<'de> Deserialize<'de> for Action {
    fn deserialize<D>(deserializer: D) -> Result<Self, D::Error>
        where D: Deserializer<'de>
    {
        let s = String::deserialize(deserializer)?;
        if s == "TURN" {
            Ok(Action::TURN)
        } else if s == "SEE" {
            Ok(Action::SEE)
        }  else if s == "WALK" {
            Ok(Action::WALK)
        }  else if s == "PICK" {
            Ok(Action::PICK)
        }  else if s == "STAND" {
            Ok(Action::STAND)
        }  else if s == "OTHER" {
            Ok(Action::OTHER)
        } else {
            Action::deserialize(s.into_deserializer())
        }
    }
}

// impl Action {
//     pub(crate) fn value(&self, prompt : String) -> String {
//         return match self {
//             Action::TURN(value) => add_action(prompt, "turned", value),
//             Action::SEE(value) => add_action(prompt, "saw", value),
//             Action::WALK(value) => add_action(prompt, "walked", value),
//             Action::PICK(value) => add_action(prompt, "picked up", value),
//             Action::STAND(value) => add_action(prompt, "stood", value),
//             Action::OTHER(value) => create_prompt(Action::OTHER("".to_string()), value),
//         }
//     }
// }

impl Action {
    pub(crate) fn value(&self) -> &str {
        return match self {
            Action::TURN => "turned",
            Action::SEE => "saw",
            Action::WALK => "walked",
            Action::PICK => "picked up",
            Action::STAND => "stood",
            Action::OTHER => ""
        }
    }
}


pub(crate) fn create_prompt(action: Action, value : &str) -> String {
    return format!("Make a 30 words, funny, a bit mean, joke about a player who: {0} {1},", action.value(), value);
}

pub(crate) fn add_action(prompt: String, action: &str, value: &str) -> String {
    return format!("{}{}", prompt, format_args!("{} {}, ", action, &value));
}

pub(crate) fn end_prompt(prompt: String) -> String {
    return prompt + "."
}