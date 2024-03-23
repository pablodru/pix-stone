import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
    stages: [
        { duration: "7s", target: 20 },
        { duration: "20s", target: 40 },
        { duration: "33s", target: 60 },
    ],
};

export default function (){

    const paymentToCreate = {
        origin:{
            user:{
                cpf: "37269767345"
            },
            account: {
                number: "811046258",
                agency: "4763"
            }
        },
        destiny: {
            key: {
                value: "72217989468",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer d175a658-153b-487f-bbbf-924301244235` };

    http.post(`http://localhost:5109/payments`, body, { headers });

    sleep(1);
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}