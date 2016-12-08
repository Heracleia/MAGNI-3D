function send_error(error)

%%  Matlab Server that waits for client request
%%  in order to execute the Dynamic Time Warping
%% (DTW)algorithm in real time.

output_port = 13133;

import java.net.ServerSocket
import java.io.*




server_socket = ServerSocket(output_port);
server_socket.setSoTimeout(1000);

output_socket = server_socket.accept;

fprintf(1, 'Client connected\n');





message = num2str(error);
output_stream   = output_socket.getOutputStream;
d_output_stream = DataOutputStream(output_stream);
% output the data over the DataOutputStream
% Convert to stream of bytes
fprintf(1, 'Writing %d bytes\n', length(message));
d_output_stream.writeBytes(char(message));
d_output_stream.flush;


% clean up
server_socket.close;
output_socket.close;
%break;


end