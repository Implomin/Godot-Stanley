pub(crate) enum Action {
    TURN(String),
    SEE(String),
    WALK(String),
    PICK(String),
    STAND(String),
    OTHER(String)
}

impl Action {
    pub(crate) fn value(&self, prompt : String) -> String {
        return match self {
            Action::TURN(value) => add_action(prompt, "turned", value),
            Action::SEE(value) => add_action(prompt, "saw", value),
            Action::WALK(value) => add_action(prompt, "walked", value),
            Action::PICK(value) => add_action(prompt, "picked up", value),
            Action::STAND(value) => add_action(prompt, "stood", value),
            Action::OTHER(value) => create_prompt("", value)
        }
    }
}

pub(crate) fn create_prompt(action: &str, value : &String) -> String {
    return format!("Make a short funny, a bit mean, narration about a player who: {0} {1},", action, value);
}

pub(crate) fn add_action(prompt: String, action: &str, value: &String) -> String {
    return prompt + action + " " + &value+ ", "
}

pub(crate) fn end_prompt(prompt: String) -> String {
    return prompt + "."
}