function matlab_server()

%%  Matlab Server that waits for client request
%%  in order to execute the Dynamic Time Warping
%% (DTW)algorithm in real time.

output_port = 13135;
number_of_retries = 100000;

import java.net.ServerSocket
import java.io.*

if (nargin < 3)
    number_of_retries = -1; % set to -1 for infinite
end
retry             = 0;

server_socket  = [];
output_socket  = [];

while true
    
    retry = retry + 1;
    
    try
        if ((number_of_retries > 0) && (retry > number_of_retries))
            fprintf(1, 'Too many retries\n');
            break;
        end
        
        fprintf(1, ['Try %d waiting for client to connect to this ' ...
            'host on port : %d\n'], retry, output_port);
        
        % wait for 1 second for client to connect server socket
        server_socket = ServerSocket(output_port);
        server_socket.setSoTimeout(1000);
        
        output_socket = server_socket.accept;
        
        fprintf(1, 'Client connected\n');
        
        input_stream   = output_socket.getInputStream;
        d_input_stream = DataInputStream(input_stream);
        
        
        % read data from the socket - wait a short time first
        pause(0.5);
        bytes_available = input_stream.available;
        fprintf(1, 'Reading %d bytes\n', bytes_available);
        
        message = zeros(1, bytes_available, 'uint8');
        for i = 1:bytes_available
            message(i) = d_input_stream.readByte;
        end
        
        message = char(message);
        
        
        % 1_2_1_6
        %patientID.ToString() + "_" + sessionID.ToString()+ "_" + playlistID.ToString() + "_" + therapist_exercise.ToString ();
        C = strsplit(message,'_')
        
        patientID = C{1};
        sessionID = C{2};
        playlistID = C{3};
        exerciseID = C{4};
        
        csv_file1 = exerciseID;
        csv_file2 = strcat('pat_', 'session', sessionID, '_playlist', playlistID, '_exercise', exerciseID);
        
        csv_file1
        csv_file2
        
        
        error = server_dtw(csv_file1, csv_file2, C)
        %% TODO sent the error back
         % TODO --> send error in client C#
       
         
        message = num2str(error);
        output_stream   = output_socket.getOutputStream;
        d_output_stream = DataOutputStream(output_stream);
        
        % output the data over the DataOutputStream
        % Convert to stream of bytes
        fprintf(1, 'Writing %d bytes\n', length(message));
        d_output_stream.writeBytes(message);
        d_output_stream.flush;
        
        
        close all;
        % clean up
        server_socket.close;
        output_socket.close;
        %break;
        
    catch
        if ~isempty(server_socket)
            server_socket.close
        end
        
        if ~isempty(output_socket)
            output_socket.close
        end
        
        % pause before retrying
        pause(1);
    end
end
end