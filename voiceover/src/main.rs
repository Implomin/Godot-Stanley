mod server;
use std::net::{TcpListener, TcpStream};
#[path = "http/refactored.rs"]
mod refactored;
fn main() {
    let listener = TcpListener::bind("127.0.0.1:5051").expect("Failed creating a listner");
    println!("LISTENING ON 127.0.0.1:5051");
    for stream in listener.incoming() {
        match stream {
            Ok(stream) => {
                std::thread::spawn(|| server::handle_client(stream));
            }
            Err(e) => {
                eprintln!("FAILED TO ESTABLISH CONNECTION: {}", e)
            }
        }
    }
}
