using System.Collections.Generic;


public class message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class function
{
    string logit_bias { get; set; }
    int max_tokens { get; set; }
}


public class OobaParameters
{
    public List<message> messages { get; set; }

    public string model { get; set; }
    public int frequency_penalty { get; set; } = 0;
    public string function_call { get; set; }

    // Deprecated in OpenAI in favor of "tools"
    // ...so this may change
    public List<function> functions { get; set; }
    // OBJECT?
    public string logit_bias { get; set; }

    public int max_tokens { get; set; } = 0;
    public int n { get; set; } = 1;
    public int presence_penalty { get; set; } = 0;
    public string stop { get; set; }
    public bool stream { get; set; } = true;
    public int temperature { get; set; } = 1;
    public int top_p { get; set; } = 1;
    public string user { get; set; }
    public string mode { get; set; } = "chat";
    public string user_input { get; set; }
    public string instruction_template { get; set; }
    public string turn_template { get; set; }
    public string name1_instruct { get; set; }
    public string name2_instruct { get; set; }
    public string context_instruct { get; set; }
    public string system_message { get; set; }
    public string character { get; set; }
    public string name1 { get; set; }
    public string name2 { get; set; }
    public string context { get; set; }
    public string greeting { get; set; }
    public string chat_instruct_command { get; set; }
    public bool continue_ { get; set; } = false;
    public string preset { get; set; }
    public float min_p { get; set; } = 0;
    public float top_k { get; set; } = 0;
    public float repetition_penalty { get; set; } = 1;
    public float repetition_penalty_range { get; set; } = 0;
    public float typical_p { get; set; } = 1;
    public float tfs { get; set; } = 1;
    public float top_a { get; set; } = 0;
    public float epsilon_cutoff { get; set; } = 0;
    public float eta_cutoff { get; set; } = 0;
    public float guidance_scale { get; set; } = 1;
    public string negative_prompt { get; set; }
    public float penalty_alpha { get; set; } = 0;
    public float mirostat_mode { get; set; } = 0;
    public float mirostat_tau { get; set; } = 5;
    public float mirostat_eta { get; set; } = 0.1f;
    public bool temperature_last { get; set; } = false;
    public bool do_sample { get; set; } = true;
    public int seed { get; set; } = -1;
    public float encoder_repetition_penalty { get; set; } = 1;
    public float no_repeat_ngram_size { get; set; } = 0;
    public int min_length { get; set; } = 0;
    public int num_beams { get; set; } = 1;
    public float length_penalty { get; set; } = 1;
    public bool early_stopping { get; set; } = false;
    public int truncation_length { get; set; } = 0;
    public int max_tokens_second { get; set; } = 0;
    public string custom_token_bans { get; set; }
    public bool auto_max_new_tokens { get; set; } = false;
    public bool ban_eos_token { get; set; } = false;
    public bool add_bos_token { get; set; } = true;
    public string grammar_string { get; set; }





}
