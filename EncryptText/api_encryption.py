from flask import Flask, jsonify, request
import sys
import enctext,dectext
from flask_cors import CORS 


app = Flask(__name__)
CORS(app, resources={r"/*": {"origins": "*"}}, supports_credentials=True)


@app.route('/')
def home():
    return jsonify(message="Welcome to the my encrype API!")

@app.route('/enctext', methods=['POST'])
def enc():
    data = request.get_json()
    res = enctext.encrypt_text(data["text"],data["key"])
    return jsonify(result = res)


@app.route('/dectext', methods=['POST'])
def dec():
    data = request.get_json()
    try:
        res = dectext.decrypt_text(data["text"],data["key"])
        return jsonify(result = res)
    except Exception as ex:
        return jsonify(result = "not working check your key")
   



def run_app(port=5220):
    print(f"Server is running on http://localhost:{port}")
    app.run(host='localhost', port=port)
    input("Press Enter to exit...")

if __name__ == '__main__':
    # בדיקת פורט מהארגומנטים של שורת הפקודה (אם הוזן)
    if len(sys.argv) > 1:
        port = int(sys.argv[1])
    else:
        port = 5220  # ברירת מחדל

    run_app(port)
