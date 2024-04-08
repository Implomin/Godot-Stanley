use serde::Deserialize;

use super::action::Action;

#[derive(Deserialize)]
pub(crate) struct Request {
    pub action: Action,
    pub value: String
}