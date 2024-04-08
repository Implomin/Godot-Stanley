use serde::Deserialize;
use serde::Serialize;
#[derive(Serialize, Deserialize)]
pub(crate) struct OllamaResponse {
    pub model: String,
    pub created_at: String,
    pub response: String,
    pub done: bool,
    pub context: Vec<i32>,
    pub total_duration: i64,
    pub load_duration: i32,
    pub prompt_eval_duration: i64,
    pub eval_count: i32,
    pub eval_duration: i64,
}
