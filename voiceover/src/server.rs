use std::{
    io::{Read, Write},
    net::TcpStream, process::{Child, Command},
};
mod action;
mod ollama_response;
mod ollama_request;
use serde_json::Result;

use crate::server::action::end_prompt;

pub(crate) fn handle_client(mut stream: TcpStream) {
    let mut buffer = [0; 1024];

    stream
        .read(&mut buffer)
        .expect("Error reading bytes from a tcp stream");

    let request = String::from_utf8_lossy(&buffer[..]);

    let body = request.split_once("\r\n\r\n").unwrap().1.trim_end_matches("\0");

    let request_body : ollama_request::Request = serde_json::from_str(body).unwrap();
    let body = action::end_prompt(action::create_prompt(request_body.action, &request_body.value));
    // let body = "What kind of sound do ducks make? Make a funny joke";
    let body_length = body.chars().count();
    // println!("BODY: {} CHARS: {:?}",&body, body_length);

    // let response3 = generate_prompt(&body);
    // println!("REQUEST: {}", request);

    let mut connection = match TcpStream::connect("127.0.0.1:11434") {
        Ok(res) => res,
        Err(e) => panic!("{}", e),
    };

    let mut buffer = [0; 1024];

    let tiny_dolphin_prompt = format!(
        "{{\"model\": \"tinydolphin\", \"prompt\": \"{}\", \"stream\":false}}",
        body
    );
    let ollama_request = format!("POST /api/generate HTTP/1.1\nHost: 127.0.0.1\nContent-Type: application/json\nContent-Length: {}\nAccept: */*\nUser-Agent: curl/7.87.0\nConnection: close\n\n{}",tiny_dolphin_prompt.len(), tiny_dolphin_prompt);

    let tiny_dolphin_prompt_bytes = ollama_request.as_bytes();

    // println!("{}", ollama_request);
    connection
        .write(&tiny_dolphin_prompt_bytes[..])
        .expect("DOES NOT WORK");

    let mut r_string = String::new();

    connection
        .read_to_string(&mut r_string)
        .expect("Could not read data from a response"); 
    let split = r_string.split_once("\r\n\r\n");

    let binding: &str = split.unwrap().1;
    let res: ollama_response::OllamaResponse =
        serde_json::from_str(&binding).expect("COULD NOT PARSE OLLAMA REPSONSE ");
    println!("{}", res.response);
    delegate_to_TTY(&res.response);
    stream
        .write(responseHttp(&res.response, "200", "application/json").as_bytes())
        .expect("Failed sending a response");
}

fn responseHttp(body : &str, code : &str, content_type : &str) -> String {
    return format!("{}", format_args!("HTTP/1.1 {}\nContent-Type: {}\n\n{{\"prompt\": \"{}\"}}", code, content_type, body.replace("\"", "\\\"")))
}
// tts --text "Example string." --model_name "tts_models/multilingual/multi-dataset/your_tts" --speaker_wav=resources/inputs/output.wav  --language_idx=en --out_path resources/outputs/speech.wav
fn delegate_to_TTY(prompt: &str) -> Child {
    Command::new("tts")
    .arg("--text")
    .arg(prompt)
    .arg("--model_name")
    .arg("tts_models/multilingual/multi-dataset/your_tts")
    .arg("--speaker_wav=resources/inputs/output.wav")
    .arg("--language_idx=en")
    .arg("--out_path")
    .arg("resources/outputs/speech.wav")
    .spawn()
    .expect("COULD NOT RUN TTY COMMAND")
}



fn generate_prompt(prompt: &str) -> String {
    print!("{}", format_args!("HTTP/1.1 200 OK\n\n{}", prompt));

    let mut connection = TcpStream::connect("127.0.0.1:11434").expect("Connection to ollma failed");

    let mut buffer = [0; 1024];

    let tiny_dolphin_prompt = format!(
        "{}{{\"model\": \"tinydolphin\", \"prompt\": \"{}\", \"stream\":false}}",
        "POST /api/generate HTTP/1.1\nHost: localhost:5051\nContent-Type: application/json\nAccept: */*\nUser-Agent: curl/7.87.0\nContent-Length: 35\n\n",
        prompt);
    let tiny_dolphin_prompt_bytes = tiny_dolphin_prompt.as_bytes();

    connection
        .write(&tiny_dolphin_prompt_bytes[..])
        .expect("Could not write data to ollama");

    let mut r_string = String::new();
    connection
        .read_to_string(&mut r_string)
        .expect("Could not read data from a response");

    println!("RESPONSE: {}", &r_string);
    connection
        .shutdown(std::net::Shutdown::Both)
        .expect("Could not close ollama connection");
    return r_string;
}
