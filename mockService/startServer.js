const mockserver = require('mockserver-node');
const mockServerClient = require('mockserver-client').mockServerClient;

const PORT = 1080;
const REQUEST_DELAY_MS = 50;

const DEFAULT_DELAY = {
    timeUnit: "MILLISECONDS",
    value: REQUEST_DELAY_MS
};

(async () => {
    await mockserver.start_mockserver({
        serverPort: PORT,
        verbose: true,
        trace: true
    });

    await mockServerClient("localhost", PORT).mockAnyResponse({
        httpRequest: {
            path: "/api/views.open"
        },
        httpResponse: {
            body: {
                property: "value"
            },
            delay: DEFAULT_DELAY
        }
    });

    await mockServerClient("localhost", PORT).mockAnyResponse({
        httpRequest: {
            path: "/api/views.update"
        },
        httpResponse: {
            body: {
                property: "value"
            },
            delay: DEFAULT_DELAY
        }
    });

    await mockServerClient("localhost", PORT).mockAnyResponse({
        httpRequest: {
            path: "/api/chat.postMessage"
        },
        httpResponse: {
            body: {
                property: "value"
            },
            delay: DEFAULT_DELAY
        }
    });

    await mockServerClient("localhost", PORT).mockAnyResponse({
        httpRequest: {
            path: "/api/chat.update"
        },
        httpResponse: {
            body: {
                property: "value"
            },
            delay: DEFAULT_DELAY
        }
    });

    await mockServerClient("localhost", PORT).mockAnyResponse({
        httpRequest: {
            path: "/api/chat.delete"
        },
        httpResponse: {
            body: {
                property: "value"
            },
            delay: DEFAULT_DELAY
        }
    });
})();